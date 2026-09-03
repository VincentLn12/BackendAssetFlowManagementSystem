
using Core.Interfaces.Specifications.AssetSubItem;
using Core.Interfaces.Specifications.AssetSubItemHistory;


namespace API.Controllers;

public class AssetSubItemHistoryController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetSubItemHistory>>> GetAssetSubItemHistory([FromQuery] AssetSubItemHistorySpecParams assetSubItemHistorySpecParams)
    {
        var spec = new AssetSubItemHistorySpecification(assetSubItemHistorySpecParams);

        var assetSubItemHistories = await unit.Repository<AssetSubItemHistory>().ListAsync(spec);
         
        var countSpec = new AssetSubItemHistorySpecification(assetSubItemHistorySpecParams);    
        var totalItems = await unit.Repository<AssetSubItemHistory>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetSubItemHistoryDto>>(assetSubItemHistories);

        return Ok(new Pagination<AssetSubItemHistoryDto>(
            assetSubItemHistorySpecParams.PageIndex,
            assetSubItemHistorySpecParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-withdrawal/{procurement_withdrawal_id}")]
    public async Task<ActionResult<Pagination<AssetSubItemHistoryDto>>> GetAssetSubItemsByAssetId(
     int procurement_withdrawal_id,
     [FromQuery] AssetSubItemHistorySpecParams assetSubItemHistorySpecParams)
    {
        assetSubItemHistorySpecParams.procurement_withdrawal_id = procurement_withdrawal_id;
        var spec = new AssetSubItemHistorySpecification(assetSubItemHistorySpecParams);

        var assetSubItems = await unit.Repository<AssetSubItemHistory>().ListAsync(spec);

        var countSpec = new AssetSubItemHistorySpecification(assetSubItemHistorySpecParams);    
        var totalItems = await unit.Repository<AssetSubItemHistory>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetSubItemHistoryDto>>(assetSubItems);

        return Ok(new Pagination<AssetSubItemHistoryDto>(
            assetSubItemHistorySpecParams.PageIndex,
            assetSubItemHistorySpecParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetSubItemHistory>> GetAssetSubItemHistory(int id)
    {
        var assetSubItemHistory = await unit.Repository<AssetSubItemHistory>().GetByIdAsync(id);

        if (assetSubItemHistory == null) return NotFound();
        return assetSubItemHistory;
    }

    [HttpPost]
    public async Task<ActionResult<AssetSubItemHistoryDto>> CreateAssetSubItemHistory(
         AssetSubItemHistoryDto dto)
        {
        var assetSubItemHistory = mapper.Map<AssetSubItemHistory>(dto);

        assetSubItemHistory.is_active = true;
        unit.Repository<AssetSubItemHistory>().Add(assetSubItemHistory);
        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetSubItemHistory),
                new { id = assetSubItemHistory.sub_item_history_id },
                assetSubItemHistory 
            );
        }

        return BadRequest("Problem creating asset sub item history");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetSubItemHistory(int id, AssetSubItemHistoryDto dto)
    {
        if (id != dto.sub_item_history_id)
            return BadRequest("Cannot update this asset sub item history");

        var existingAssetSubItemHistory = await unit.Repository<AssetSubItemHistory>().GetByIdAsync(id);

        if (existingAssetSubItemHistory == null)
            return NotFound("Asset sub item history not found");

        mapper.Map(dto, existingAssetSubItemHistory);

        existingAssetSubItemHistory.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset sub item history");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetSubItemHistory(int id)
    {
        var assetSubItemHistory = await unit.Repository<AssetSubItemHistory>().GetByIdAsync(id);

        if (assetSubItemHistory == null) return NotFound();
        assetSubItemHistory.is_active = false;
        assetSubItemHistory.updated_at = DateTime.UtcNow;

        unit.Repository<AssetSubItemHistory>().Update(assetSubItemHistory);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset sub item history");
    }

}
