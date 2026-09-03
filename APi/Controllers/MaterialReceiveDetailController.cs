
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

        var procurementRecord = await unit.Repository<Procurement_records>()
             .GetByIdAsync(materialReceiveDetail.procurement_record_id);

        if (procurementRecord == null)
            return BadRequest("Procurement record not found");

        var currentBalance = await GetLatestBalance(
            materialReceiveDetail.material_item_id,
            procurementRecord.department_id
        );
        var newBalance = currentBalance + materialReceiveDetail.quantity;

        unit.Repository<MaterialReceiveDetail>().Add(materialReceiveDetail);

        if (!await unit.Complete())
            return BadRequest("Problem creating material receive detail");

        // เพิ่ม Stock Card / Transaction
        var stockCard = new MaterialStockCard
        {
            material_item_id = materialReceiveDetail.material_item_id,
            procurement_record_id = materialReceiveDetail.procurement_record_id,
            department_id = procurementRecord.department_id,
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

        if (!await unit.Complete())
            return BadRequest("Problem creating stock transaction");

        await RecalculateStockCards(materialReceiveDetail.material_item_id, procurementRecord.department_id);
        await SyncMaterialItemFromStockCards(materialReceiveDetail.material_item_id);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetMaterialReceiveDetail),
                new { id = materialReceiveDetail.receive_detail_id },
                mapper.Map<MaterialReceiveDetailDto>(materialReceiveDetail)
            );
        }

        return BadRequest("Problem updating stock balances");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialReceiveDetail(int id, MaterialReceiveDetailDto dto)
    {
        if (id != dto.receive_detail_id)
            return BadRequest("Cannot update this material receive detail");

        var existing = await unit.Repository<MaterialReceiveDetail>().GetByIdAsync(id);

        if (existing == null)
            return NotFound("Material receive detail not found");

        if (!existing.is_active)
            return BadRequest("ไม่สามารถแก้ไขรายการที่ถูกลบแล้ว");

        var oldMaterialItemId = existing.material_item_id;
        var oldProcurementRecordId = existing.procurement_record_id;

        var oldProcurementRecord = await unit.Repository<Procurement_records>()
            .GetByIdAsync(oldProcurementRecordId);

        mapper.Map(dto, existing);

        existing.total_amount = existing.quantity * existing.unit_price;
        existing.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialReceiveDetail>().Update(existing);

        await UpdateReceiveStockCard(existing);

        if (!await unit.Complete())
            return BadRequest("Problem updating the material receive detail");

        await RecalculateStockCards(oldMaterialItemId, oldProcurementRecord?.department_id);
        if (
            oldMaterialItemId != existing.material_item_id ||
            oldProcurementRecordId != existing.procurement_record_id
        )
        {
            var newProcurementRecord = await unit.Repository<Procurement_records>()
                .GetByIdAsync(existing.procurement_record_id);
            await RecalculateStockCards(existing.material_item_id, newProcurementRecord?.department_id);
        }

        await SyncMaterialItemFromStockCards(oldMaterialItemId);
        await SyncMaterialItemFromStockCards(existing.material_item_id);

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating stock balances");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialReceiveDetail(int id)
    {
        var materialReceiveDetail = await unit.Repository<MaterialReceiveDetail>().GetByIdAsync(id);

        if (materialReceiveDetail == null) return NotFound();
        if (!materialReceiveDetail.is_active)
            return BadRequest("รายการนี้ถูกลบไปแล้ว");

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
            balance_qty = 0,
            unit_price = materialReceiveDetail.unit_price,
            total_amount = 0,
            fiscal_year_id = procurementRecord?.fiscal_year_id,
            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(cancelStockCard);

        if (!await unit.Complete())
            return BadRequest("Problem deleting material receive detail");

        await RecalculateStockCards(materialReceiveDetail.material_item_id, procurementRecord?.department_id);
        await SyncMaterialItemFromStockCards(materialReceiveDetail.material_item_id);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem updating stock balances");
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
    private async Task UpdateReceiveStockCard(MaterialReceiveDetail receiveDetail)
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
                procurement_record_id = receiveDetail.procurement_record_id,
                transaction_date = DateTime.UtcNow,
                transaction_type = "IN",
                receive_detail_id = receiveDetail.receive_detail_id,
                quantity_in = receiveDetail.quantity,
                quantity_out = 0,
                balance_qty = 0,
                unit_price = receiveDetail.unit_price,
                total_amount = 0,
                is_active = true,
                created_at = DateTime.UtcNow
            };
        }

        var procurementRecord = await unit.Repository<Procurement_records>()
                 .GetByIdAsync(receiveDetail.procurement_record_id);

        stockCard.material_item_id = receiveDetail.material_item_id;
        stockCard.procurement_record_id = receiveDetail.procurement_record_id;
        stockCard.department_id = procurementRecord?.department_id;
        stockCard.transaction_date = DateTime.UtcNow;
        stockCard.reference_document_no = procurementRecord?.document_no;
        stockCard.quantity_in = receiveDetail.quantity;
        stockCard.quantity_out = 0;
        stockCard.balance_qty = 0;
        stockCard.unit_price = receiveDetail.unit_price;
        stockCard.total_amount = 0;
        stockCard.fiscal_year_id = procurementRecord?.fiscal_year_id;
        stockCard.updated_at = DateTime.UtcNow;

        if (stockCard.stock_card_id == 0)
        {
            unit.Repository<MaterialStockCard>().Add(stockCard);
            return;
        }

        unit.Repository<MaterialStockCard>().Update(stockCard);
    }

    private async Task<decimal> GetLatestBalance(int materialItemId, int? departmentId)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        return stockCards
            .Where(x =>
                x.is_active &&
                x.material_item_id == materialItemId &&
                x.department_id == departmentId
            )
            .OrderByDescending(x => x.transaction_date)
            .ThenByDescending(x => x.stock_card_id)
            .Select(x => x.balance_qty)
            .FirstOrDefault();
    }

    private async Task RecalculateStockCards(int materialItemId, int? departmentId)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var itemStockCards = stockCards
            .Where(x =>
                x.is_active &&
                x.material_item_id == materialItemId &&
                x.department_id == departmentId
            )
            .OrderBy(x => x.transaction_date)
            .ThenBy(x => x.stock_card_id)
            .ToList();

        decimal runningBalance = 0;

        foreach (var stockCard in itemStockCards)
        {
            runningBalance += stockCard.quantity_in - stockCard.quantity_out;
            stockCard.balance_qty = runningBalance;
            stockCard.total_amount = runningBalance * stockCard.unit_price;
            stockCard.updated_at = DateTime.UtcNow;
            unit.Repository<MaterialStockCard>().Update(stockCard);
        }
    }

    private async Task SyncMaterialItemFromStockCards(int materialItemId)
    {
        var materialItem = await unit.Repository<MaterialItem>().GetByIdAsync(materialItemId);
        if (materialItem == null)
            return;

        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var itemStockCards = stockCards
            .Where(x => x.is_active && x.material_item_id == materialItemId)
            .OrderBy(x => x.transaction_date)
            .ThenBy(x => x.stock_card_id)
            .ToList();

        var latestStockCard = itemStockCards.LastOrDefault();
        var groupedBalances = itemStockCards
            .GroupBy(x => x.department_id)
            .Select(g => g
                .OrderByDescending(x => x.transaction_date)
                .ThenByDescending(x => x.stock_card_id)
                .First()
                .balance_qty)
            .ToList();

        materialItem.quantity_in = itemStockCards.Sum(x => x.quantity_in);
        materialItem.quantity_out = itemStockCards.Sum(x => x.quantity_out);
        materialItem.unit_price = latestStockCard?.unit_price ?? materialItem.unit_price;
        var currentBalance = groupedBalances.Sum();
        materialItem.total_amount = currentBalance * (materialItem.unit_price ?? 0);
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);
    }

}
