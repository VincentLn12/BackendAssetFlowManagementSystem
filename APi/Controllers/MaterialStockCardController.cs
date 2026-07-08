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

}
