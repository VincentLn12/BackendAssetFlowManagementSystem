
using Core.Interfaces.Specifications.AssetRepair;
using Core.Interfaces.Specifications.MaterialIssueDetail;
using Core.Interfaces.Specifications.MaterialReceiveDetail;


namespace API.Controllers;

public class MaterialReceiveDetailController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaterialReceiveDetailDto>>> GetMaterialReceiveDetail([FromQuery] MaterialReceiveDetailSpecParams materialReceiveDetailParams)
    {
        var spec = new MaterialReceiveDetailSpecification(materialReceiveDetailParams);

        var materialReceiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAsync(spec);
        var countSpec = new MaterialReceiveDetailSpecification(materialReceiveDetailParams);
        var totalItems = await unit.Repository<MaterialReceiveDetail>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<MaterialReceiveDetailDto>>(materialReceiveDetails);
        return Ok(new Pagination<MaterialReceiveDetailDto>(
            materialReceiveDetailParams.PageIndex,
            materialReceiveDetailParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-procurement-record/{procurement_record_id:int}")]
    public async Task<ActionResult<Pagination<MaterialReceiveDetailDto>>> GetMaterialReceiveDetailsByProcurementRecordId(
      int procurement_record_id,
      [FromQuery] MaterialReceiveDetailSpecParams materialReceiveDetailParams)
    {
        materialReceiveDetailParams.procurement_record_id = procurement_record_id;
        var spec = new MaterialReceiveDetailSpecification(materialReceiveDetailParams);
        var materialReceiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAsync(spec);

        var countSpec = new MaterialReceiveDetailSpecification(materialReceiveDetailParams);

        var totalItems = await unit.Repository<MaterialReceiveDetail>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<MaterialReceiveDetailDto>>(materialReceiveDetails);

        return Ok(new Pagination<MaterialReceiveDetailDto>(
            materialReceiveDetailParams.PageIndex,
            materialReceiveDetailParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialReceiveDetailDto>> GetMaterialReceiveDetail(int id)
    {
        var materialReceiveDetail = await unit.Repository<MaterialReceiveDetail>().GetByIdAsync(id);

        if (materialReceiveDetail == null) return NotFound();
        return mapper.Map<MaterialReceiveDetailDto>(materialReceiveDetail);
    }


    [HttpPost]
    public async Task<ActionResult<MaterialReceiveDetailDto>> CreateMaterialReceiveDetail(MaterialReceiveDetailDto dto)
    {
        var materialReceiveDetail = mapper.Map<MaterialReceiveDetail>(dto);
        // หา item_no ล่าสุดของเอกสารนี้
        var allReceiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAllAsync();

        var lastItemNo = allReceiveDetails
     .Where(x =>
         x.procurement_record_id == materialReceiveDetail.procurement_record_id &&
         x.is_active)
     .Select(x => x.item_no)
     .DefaultIfEmpty(0)
     .Max();

        materialReceiveDetail.item_no = lastItemNo + 1;

        materialReceiveDetail.is_active = true;
        materialReceiveDetail.total_amount = materialReceiveDetail.quantity * materialReceiveDetail.unit_price;

        unit.Repository<MaterialReceiveDetail>().Add(materialReceiveDetail);

        // บันทึกรับเข้าก่อน เพื่อให้ได้ receive_detail_id
        if (!await unit.Complete())
            return BadRequest("Problem creating material receive detail");

        // ดึงวัสดุมาอัปเดตยอด
        var materialItem = await unit.Repository<MaterialItem>()
            .GetByIdAsync(materialReceiveDetail.material_item_id);

        if (materialItem == null)
            return BadRequest("Material item not found");

        var oldBalance = materialItem.current_balance ?? 0;
        var newBalance = oldBalance + materialReceiveDetail.quantity;

        // อัปเดตยอดวัสดุ
        materialItem.quantity_in = (materialItem.quantity_in ?? 0) + materialReceiveDetail.quantity;
        materialItem.current_balance = newBalance;
        materialItem.unit_price = materialReceiveDetail.unit_price;
        materialItem.total_amount = newBalance * materialReceiveDetail.unit_price;
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);

        var procurementRecord = await unit.Repository<Procurement_records>()
             .GetByIdAsync(materialReceiveDetail.procurement_record_id);

        if (procurementRecord == null)
            return BadRequest("Procurement record not found");

        // เพิ่ม Stock Card / Transaction
        var stockCard = new MaterialStockCard
        {
            material_item_id = materialReceiveDetail.material_item_id,
            transaction_date = DateTime.UtcNow,
            transaction_type = "IN",
            reference_document_no = procurementRecord.document_no,
            receive_detail_id = materialReceiveDetail.receive_detail_id,
            issue_detail_id = null,

            quantity_in = materialReceiveDetail.quantity,
            quantity_out = 0,
            balance_qty = newBalance,
            unit_price = materialReceiveDetail.unit_price,
            total_amount = newBalance * materialReceiveDetail.unit_price,
            fiscal_year_id = procurementRecord.fiscal_year_id,

            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(stockCard);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetMaterialReceiveDetail),
                new { id = materialReceiveDetail.receive_detail_id },
                mapper.Map<MaterialReceiveDetailDto>(materialReceiveDetail)
            );
        }

        return BadRequest("Problem creating stock transaction");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialReceiveDetail(int id, MaterialReceiveDetailDto dto)
    {
        if (id != dto.receive_detail_id)
            return BadRequest("Cannot update this material receive detail");

        var existing = await unit.Repository<MaterialReceiveDetail>().GetByIdAsync(id);

        if (existing == null)
            return NotFound("Material receive detail not found");

        // เก็บค่าเดิมก่อนแก้
        var oldMaterialItemId = existing.material_item_id;
        var oldQuantity = existing.quantity;

        // map ค่าใหม่
        mapper.Map(dto, existing);

        existing.total_amount = existing.quantity * existing.unit_price;
        existing.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialReceiveDetail>().Update(existing);

        // กรณีแก้วัสดุตัวเดิม
        if (oldMaterialItemId == existing.material_item_id)
        {
            var materialItem = await unit.Repository<MaterialItem>().GetByIdAsync(existing.material_item_id);

            if (materialItem == null)
                return BadRequest("Material item not found");

            var diffQty = existing.quantity - oldQuantity;

            materialItem.quantity_in = (materialItem.quantity_in ?? 0) + diffQty;
            materialItem.current_balance = (materialItem.current_balance ?? 0) + diffQty;
            materialItem.unit_price = existing.unit_price;
            materialItem.total_amount = (materialItem.current_balance ?? 0) * existing.unit_price;
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);

            await UpdateReceiveStockCard(existing, materialItem.current_balance ?? 0);
        }
        else
        {
            // กรณีเปลี่ยนรายการวัสดุ ต้องลบยอดออกจากตัวเก่า และเพิ่มให้ตัวใหม่
            var oldMaterial = await unit.Repository<MaterialItem>().GetByIdAsync(oldMaterialItemId);
            var newMaterial = await unit.Repository<MaterialItem>().GetByIdAsync(existing.material_item_id);

            if (oldMaterial == null || newMaterial == null)
                return BadRequest("Material item not found");

            oldMaterial.quantity_in = (oldMaterial.quantity_in ?? 0) - oldQuantity;
            oldMaterial.current_balance = (oldMaterial.current_balance ?? 0) - oldQuantity;
            oldMaterial.total_amount = (oldMaterial.current_balance ?? 0) * (oldMaterial.unit_price ?? 0);
            oldMaterial.updated_at = DateTime.UtcNow;

            newMaterial.quantity_in = (newMaterial.quantity_in ?? 0) + existing.quantity;
            newMaterial.current_balance = (newMaterial.current_balance ?? 0) + existing.quantity;
            newMaterial.unit_price = existing.unit_price;
            newMaterial.total_amount = (newMaterial.current_balance ?? 0) * existing.unit_price;
            newMaterial.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(oldMaterial);
            unit.Repository<MaterialItem>().Update(newMaterial);

            await UpdateReceiveStockCard(existing, newMaterial.current_balance ?? 0);
        }

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the material receive detail");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialReceiveDetail(int id)
    {
        var materialReceiveDetail = await unit.Repository<MaterialReceiveDetail>().GetByIdAsync(id);

        if (materialReceiveDetail == null) return NotFound();
        if (!materialReceiveDetail.is_active)
            return BadRequest("รายการนี้ถูกลบไปแล้ว");

        var materialItem = await unit.Repository<MaterialItem>()
            .GetByIdAsync(materialReceiveDetail.material_item_id);

        if (materialItem == null)
            return BadRequest("Material item not found");

        var oldBalance = materialItem.current_balance ?? 0;
        var newBalance = oldBalance - materialReceiveDetail.quantity;

        if (newBalance < 0)
            newBalance = 0;

        materialItem.quantity_in = (materialItem.quantity_in ?? 0) - materialReceiveDetail.quantity;
        if (materialItem.quantity_in < 0)
            materialItem.quantity_in = 0;

        materialItem.current_balance = newBalance;
        materialItem.total_amount = newBalance * (materialReceiveDetail.unit_price ?? 0);
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);

        materialReceiveDetail.is_active = false;
        materialReceiveDetail.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialReceiveDetail>().Update(materialReceiveDetail);

        var procurementRecord = await unit.Repository<Procurement_records>()
            .GetByIdAsync(materialReceiveDetail.procurement_record_id);

        var cancelStockCard = new MaterialStockCard
        {
            material_item_id = materialReceiveDetail.material_item_id,
            procurement_record_id = materialReceiveDetail.procurement_record_id,
            department_id = procurementRecord?.department_id,
            transaction_date = DateTime.UtcNow,
            transaction_type = "CANCEL_IN",
            reference_document_no = procurementRecord?.document_no,
            receive_detail_id = materialReceiveDetail.receive_detail_id,
            issue_detail_id = null,
            quantity_in = 0,
            quantity_out = materialReceiveDetail.quantity,
            balance_qty = newBalance,
            unit_price = materialReceiveDetail.unit_price,
            total_amount = newBalance * (materialReceiveDetail.unit_price ?? 0),
            fiscal_year_id = procurementRecord?.fiscal_year_id,
            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(cancelStockCard);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting material receive detail");
    }

    [HttpGet("stock-card/{material_item_id:int}")]
    public async Task<ActionResult<IReadOnlyList<MaterialStockCardDto>>> GetStockCardByMaterialItem(
    int material_item_id
)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var data = stockCards
            .Where(x => x.material_item_id == material_item_id && x.is_active)
            .OrderBy(x => x.transaction_date)
            .ThenBy(x => x.stock_card_id)
            .Select(x => new MaterialStockCardDto
            {
                stock_card_id = x.stock_card_id,
                material_item_id = x.material_item_id,
                transaction_date = x.transaction_date,
                transaction_type = x.transaction_type,
                reference_document_no = x.reference_document_no,
                quantity_in = x.quantity_in,
                quantity_out = x.quantity_out,
                balance_qty = x.balance_qty,
                unit_price = x.unit_price,
                total_amount = x.total_amount,
                procurement_record_id = x.procurement_record_id,
                staff_name = x.StaffName,
                fiscal_year_id = x.fiscal_year_id
            })
            .ToList();

        return Ok(data);
    }
    private async Task UpdateReceiveStockCard(MaterialReceiveDetail receiveDetail, decimal balanceQty)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var stockCard = stockCards.FirstOrDefault(x =>
            x.receive_detail_id == receiveDetail.receive_detail_id &&
            x.transaction_type == "IN" &&
            x.is_active
        );

        if (stockCard == null)
        {
            stockCard = new MaterialStockCard
            {
                material_item_id = receiveDetail.material_item_id,
                transaction_date = DateTime.UtcNow,
                transaction_type = "IN",
                reference_document_no = receiveDetail.procurement_record_id.ToString(),
                receive_detail_id = receiveDetail.receive_detail_id,
                quantity_in = receiveDetail.quantity,
                quantity_out = 0,
                balance_qty = balanceQty,
                unit_price = receiveDetail.unit_price,
                total_amount = balanceQty * receiveDetail.unit_price,
                is_active = true,
                created_at = DateTime.UtcNow
            };

            unit.Repository<MaterialStockCard>().Add(stockCard);
            return;
        }

        var procurementRecord = await unit.Repository<Procurement_records>()
                 .GetByIdAsync(receiveDetail.procurement_record_id);

        stockCard.material_item_id = receiveDetail.material_item_id;
        stockCard.transaction_date = DateTime.UtcNow;
        stockCard.reference_document_no =procurementRecord?.document_no;
        stockCard.quantity_in = receiveDetail.quantity;
        stockCard.quantity_out = 0;
        stockCard.balance_qty = balanceQty;
        stockCard.unit_price = receiveDetail.unit_price;
        stockCard.total_amount = balanceQty * receiveDetail.unit_price;
        stockCard.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialStockCard>().Update(stockCard);
    }

}
