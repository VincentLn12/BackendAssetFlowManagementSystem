namespace API.Controllers;

public class MaterialStockCardController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet("{material_item_id:int}")]
    public async Task<ActionResult<IReadOnlyList<MaterialStockCardDto>>> GetStockCardByMaterialItem(
     int material_item_id,
     [FromQuery] int? fiscal_year_id,
     [FromQuery] int? department_id
 )
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var data = stockCards
            .Where(x =>
                x.material_item_id == material_item_id &&
                x.is_active &&
                (!fiscal_year_id.HasValue || x.fiscal_year_id == fiscal_year_id.Value) &&
                (!department_id.HasValue || x.department_id == department_id.Value)
            )
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
                fiscal_year_id = x.fiscal_year_id,
                department_id = x.department_id
            })
            .ToList();

        return Ok(data);
    }

    [HttpPost("rebuild")]
    public async Task<ActionResult> RebuildMaterialStockCards(
        [FromQuery] int? material_item_id,
        [FromQuery] int? department_id
    )
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var receiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAllAsync();
        var issueDetails = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var procurementRecords = await unit.Repository<Procurement_records>().ListAllAsync();
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var materialItems = await unit.Repository<MaterialItem>().ListAllAsync();

        var filteredStockCards = stockCards
            .Where(x =>
                x.is_active &&
                (!material_item_id.HasValue || x.material_item_id == material_item_id.Value) &&
                (!department_id.HasValue || x.department_id == department_id.Value)
            )
            .ToList();

        foreach (var stockCard in filteredStockCards)
        {
            var receiveDetail = stockCard.receive_detail_id.HasValue
                ? receiveDetails.FirstOrDefault(x => x.receive_detail_id == stockCard.receive_detail_id.Value)
                : null;
            var issueDetail = stockCard.issue_detail_id.HasValue
                ? issueDetails.FirstOrDefault(x => x.issue_detail_id == stockCard.issue_detail_id.Value)
                : null;

            var procurementRecordId = stockCard.procurement_record_id
                ?? receiveDetail?.procurement_record_id
                ?? issueDetail?.procurement_record_id;

            var procurementRecord = procurementRecordId.HasValue
                ? procurementRecords.FirstOrDefault(x => x.procurement_record_id == procurementRecordId.Value)
                : null;

            stockCard.procurement_record_id = procurementRecordId;
            stockCard.department_id = procurementRecord?.department_id ?? stockCard.department_id;
            stockCard.fiscal_year_id = procurementRecord?.fiscal_year_id ?? stockCard.fiscal_year_id;
            stockCard.reference_document_no = procurementRecord?.document_no ?? stockCard.reference_document_no;

            if (issueDetail?.staff_id.HasValue == true)
            {
                var staff = staffs.FirstOrDefault(x => x.staff_id == issueDetail.staff_id.Value);
                if (staff != null)
                {
                    stockCard.StaffName = $"{staff.first_name ?? ""} {staff.last_name ?? ""}".Trim();
                }
            }

            if (issueDetail != null)
            {
                stockCard.material_item_id = issueDetail.material_item_id;
                stockCard.unit_price = issueDetail.unit_price;
            }

            if (receiveDetail != null)
            {
                stockCard.material_item_id = receiveDetail.material_item_id;
                stockCard.unit_price = receiveDetail.unit_price;
            }

            stockCard.updated_at = DateTime.UtcNow;
            unit.Repository<MaterialStockCard>().Update(stockCard);
        }

        var groups = filteredStockCards
            .GroupBy(x => new { x.material_item_id, x.department_id })
            .ToList();

        foreach (var group in groups)
        {
            decimal runningBalance = 0;
            var ordered = group
                .OrderBy(x => x.transaction_date)
                .ThenBy(x => x.stock_card_id)
                .ToList();

            foreach (var stockCard in ordered)
            {
                runningBalance += stockCard.quantity_in - stockCard.quantity_out;
                stockCard.balance_qty = runningBalance;
                stockCard.total_amount = runningBalance * stockCard.unit_price;
                stockCard.updated_at = DateTime.UtcNow;
                unit.Repository<MaterialStockCard>().Update(stockCard);
            }
        }

        var affectedMaterialItemIds = filteredStockCards
            .Select(x => x.material_item_id)
            .Distinct()
            .ToList();

        foreach (var materialItemId in affectedMaterialItemIds)
        {
            var materialItem = materialItems.FirstOrDefault(x => x.material_item_id == materialItemId);
            if (materialItem == null)
                continue;

            var itemStockCards = stockCards
                .Where(x =>
                    x.is_active &&
                    x.material_item_id == materialItemId &&
                    (!department_id.HasValue || x.department_id == department_id.Value)
                )
                .OrderBy(x => x.transaction_date)
                .ThenBy(x => x.stock_card_id)
                .ToList();

            var latestStockCard = itemStockCards.LastOrDefault();
            var currentBalance = itemStockCards
                .GroupBy(x => x.department_id)
                .Select(g => g
                    .OrderByDescending(x => x.transaction_date)
                    .ThenByDescending(x => x.stock_card_id)
                    .First()
                    .balance_qty)
                .Sum();

            materialItem.quantity_in = itemStockCards.Sum(x => x.quantity_in);
            materialItem.quantity_out = itemStockCards.Sum(x => x.quantity_out);
            materialItem.unit_price = latestStockCard?.unit_price ?? materialItem.unit_price;
            materialItem.total_amount = currentBalance * (materialItem.unit_price ?? 0);
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);
        }

        if (!await unit.Complete())
            return BadRequest("Problem rebuilding material stock cards");

        return Ok(new
        {
            message = "Rebuild material stock cards completed",
            stock_card_count = filteredStockCards.Count,
            material_item_count = affectedMaterialItemIds.Count
        });
    }

}
