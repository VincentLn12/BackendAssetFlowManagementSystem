using Core.Interfaces.Specifications.MaterialItem;

namespace API.Controllers;

public class MaterialItemController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<MaterialItemDto>>> GetMaterialItems(
       [FromQuery] MaterialItemSpecParams materialItemParams
   )
    {
        var spec = new MaterialItemSpecification(materialItemParams);
        var materialItems = await unit.Repository<MaterialItem>().ListAsync(spec);

        var countSpec = new MaterialItemSpecification(materialItemParams);
        var totalItems = await unit.Repository<MaterialItem>().CountAsync(countSpec);

        var data = mapper.Map<List<MaterialItemDto>>(materialItems);

        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var scopedStockCards = stockCards
            .Where(x =>
                x.is_active &&
                (
                    !materialItemParams.FiscalYearId.HasValue ||
                    x.fiscal_year_id == materialItemParams.FiscalYearId.Value
                )
            )
            .ToList();

        foreach (var item in data)
        {
            var itemStockCards = scopedStockCards
                .Where(x => x.material_item_id == item.material_item_id)
                .OrderBy(x => x.transaction_date)
                .ThenBy(x => x.stock_card_id)
                .ToList();

            var quantityIn = itemStockCards.Sum(x => x.quantity_in);
            var quantityOut = itemStockCards.Sum(x => x.quantity_out);
            var lastStockCard = itemStockCards.LastOrDefault();
            var currentBalance = itemStockCards
                .GroupBy(x => x.department_id)
                .Select(g => g
                    .OrderByDescending(x => x.transaction_date)
                    .ThenByDescending(x => x.stock_card_id)
                    .First()
                    .balance_qty)
                .Sum();

            item.quantity_in = quantityIn;
            item.quantity_out = quantityOut;
            item.current_balance = currentBalance;
            item.unit_price = lastStockCard?.unit_price ?? item.unit_price;
            item.total_amount = item.current_balance * item.unit_price;
        }

        return Ok(new Pagination<MaterialItemDto>(
            materialItemParams.PageIndex,
            materialItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-department/{departmentId:int}")]
    public async Task<ActionResult<List<MaterialItemDto>>> GetMaterialItemsByDepartment(
      int departmentId,
      [FromQuery] int? fiscal_year_id,
      [FromQuery] string? search
  )
    {
        var spec = new MaterialItemSpecification(new MaterialItemSpecParams
        {
            PageIndex = 1,
            PageSize = 1000
        });

        var materialItems = await unit.Repository<MaterialItem>().ListAsync(spec);
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var stockCardsByDepartment = stockCards
            .Where(x =>
                x.is_active &&
                x.department_id == departmentId &&
                (!fiscal_year_id.HasValue || x.fiscal_year_id == fiscal_year_id.Value)
            )
            .ToList();

        var data = materialItems
            .Where(x =>
                x.is_active &&
                (
                    string.IsNullOrWhiteSpace(search) ||
                    x.material_name.Contains(search) ||
                    x.material_code.Contains(search)
                )
            )
            .Select(item =>
            {
                var itemStockCards = stockCardsByDepartment
                    .Where(sc => sc.material_item_id == item.material_item_id)
                    .OrderBy(sc => sc.transaction_date)
                    .ThenBy(sc => sc.stock_card_id)
                    .ToList();

                var quantityIn = itemStockCards.Sum(x => x.quantity_in);
                var quantityOut = itemStockCards.Sum(x => x.quantity_out);

                var lastStockCard = itemStockCards
                    .OrderByDescending(x => x.transaction_date)
                    .ThenByDescending(x => x.stock_card_id)
                    .FirstOrDefault();

                var balance = lastStockCard?.balance_qty ?? 0;
                var unitPrice = lastStockCard?.unit_price ?? item.unit_price;

                return new MaterialItemDto
                {
                    material_item_id = item.material_item_id,
                    material_code = item.material_code,
                    material_name = item.material_name,
                    unit_price = unitPrice,
                    unit_id = item.unit_id,
                    unit_name = item.Unit != null ? item.Unit.unit_name : null,
                    quantity_in = quantityIn,
                    quantity_out = quantityOut,
                    current_balance = balance,
                    total_amount = balance * unitPrice
                };
            })
            .Where(x => x.current_balance > 0 || x.quantity_in > 0 || x.quantity_out > 0)
            .ToList();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialItemDto>> GetMaterialItem(int id)
    {
        var materialItem = await unit.Repository<MaterialItem>().GetByIdAsync(id);

        if (materialItem == null) return NotFound();
        return mapper.Map<MaterialItemDto>(materialItem);
    }

    [HttpPost]
    public async Task<ActionResult<MaterialItemDto>> CreateMaterialItem(MaterialItemDto dto)
    {
        var materialItems = await unit.Repository<MaterialItem>().ListAllAsync();

        var materialItem = mapper.Map<MaterialItem>(dto);
        materialItem.is_active = true;
        materialItem.created_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Add(materialItem);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetMaterialItem),
                new { id = materialItem.material_item_id },
                mapper.Map<MaterialItemDto>(materialItem)
            );
        }

        return BadRequest("Problem creating material item");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialItem(int id, MaterialItemDto dto)
    {
        if (id != dto.material_item_id)
            return BadRequest("Cannot update this material item");

        var existingMaterialItem = await unit.Repository<MaterialItem>().GetByIdAsync(id);

        if (existingMaterialItem == null)
            return NotFound("Material item not found");

        mapper.Map(dto, existingMaterialItem);

        existingMaterialItem.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the material item");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialItem(int id)
    {
        var materialItem = await unit.Repository<MaterialItem>().GetByIdAsync(id);

        if (materialItem == null) return NotFound();
        materialItem.is_active = false;
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting material item");
    }

    [HttpPost("{id}/copy")]
    public async Task<ActionResult<MaterialItemDto>> CopyMaterialItem(int id)
    {
        var existingMaterialItem = await unit.Repository<MaterialItem>()
            .GetByIdAsync(id);

        if (existingMaterialItem == null)
            return NotFound("Material item not found");

        var newMaterialItem = new MaterialItem
        {
            material_code = existingMaterialItem.material_code + "-COPY",
            material_name = existingMaterialItem.material_name + " (Copy)",
            specification = existingMaterialItem.specification,
            unit_id = existingMaterialItem.unit_id,

            opening_balance = 0,
            quantity_in = 0,
            quantity_out = 0,
            unit_price = existingMaterialItem.unit_price,
            total_amount = 0,

            remark = existingMaterialItem.remark,
            min_stock = existingMaterialItem.min_stock,

            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialItem>().Add(newMaterialItem);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetMaterialItem),
                new { id = newMaterialItem.material_item_id },
                mapper.Map<MaterialItemDto>(newMaterialItem)
            );
        }

        return BadRequest("Problem copying material item");
    }
}
