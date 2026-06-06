
using Core.Interfaces.Specifications.AssetRepair;
using Core.Interfaces.Specifications.MaterialItem;


namespace API.Controllers;

public class MaterialItemController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaterialItemDto>>> GetMaterialItems([FromQuery] MaterialItemSpecParams materialItemParams)
    {
        var spec = new MaterialItemSpecification(materialItemParams);

        var materialItems = await unit.Repository<MaterialItem>().ListAsync(spec);
         
        var countSpec = new MaterialItemSpecification(materialItemParams);    
        var totalItems = await unit.Repository<MaterialItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<MaterialItemDto>>(materialItems);

        return Ok(new Pagination<MaterialItemDto>(
            materialItemParams.PageIndex,
            materialItemParams.PageSize,
            totalItems,
            data
        ));
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

}
