
using Core.Interfaces.Specifications.AssetSubItem;


namespace API.Controllers;

public class AssetsubItemController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetSubItem>>> GetAssetSubItems([FromQuery] AssetSubItemSpecParams assetSubItemParams)
    {
        var spec = new AssetSubItemSpecification(assetSubItemParams);

        var assetSubItems = await unit.Repository<AssetSubItem>().ListAsync(spec);
         
        var countSpec = new AssetSubItemForCountSpecification(assetSubItemParams);    
        var totalItems = await unit.Repository<AssetSubItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetSubItemDto>>(assetSubItems);

        return Ok(new Pagination<AssetSubItemDto>(
            assetSubItemParams.PageIndex,
            assetSubItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-assetitem/{asset_id}")]
    public async Task<ActionResult<Pagination<AssetSubItemDto>>> GetAssetSubItemsByAssetId(
      int asset_id,
      [FromQuery] AssetSubItemSpecParams assetSubItemParams)
    {
        assetSubItemParams.Asset_id = asset_id;
        var spec = new AssetSubItemSpecification(assetSubItemParams);

        var assetSubItems = await unit.Repository<AssetSubItem>().ListAsync(spec);

        var countSpec = new AssetSubItemForCountSpecification(assetSubItemParams);

        var totalItems = await unit.Repository<AssetSubItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetSubItemDto>>(assetSubItems);

        return Ok(new Pagination<AssetSubItemDto>(
            assetSubItemParams.PageIndex,
            assetSubItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetSubItem>> GetAssetSubItem(int id)
    {
        var assetSubItem = await unit.Repository<AssetSubItem>().GetByIdAsync(id);

        if (assetSubItem == null) return NotFound();
        return assetSubItem;
    }

    [HttpPost]
    public async Task<ActionResult<AssetSubItemCreateDto>> CreateAssetSubItem(
         AssetSubItemCreateDto dto)
        {
        var assetSubItem = mapper.Map<AssetSubItem>(dto);

        assetSubItem.is_active = true;

        unit.Repository<AssetSubItem>().Add(assetSubItem);
        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetSubItem),
                new { id = assetSubItem.asset_sub_item_id },
                assetSubItem
            );
        }

        return BadRequest("Problem creating project");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetSubItem(int id, AssetSubItemCreateDto dto)
    {
        if (id != dto.asset_sub_item_id)
            return BadRequest("Cannot update this asset sub item");

        var existingAssetSubItem = await unit.Repository<AssetSubItem>().GetByIdAsync(id);

        if (existingAssetSubItem == null)
            return NotFound("Asset sub item not found");

        mapper.Map(dto, existingAssetSubItem);

        existingAssetSubItem.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset sub item");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetSubItem(int id)
    {
        var assetSubItem = await unit.Repository<AssetSubItem>().GetByIdAsync(id);

        if (assetSubItem == null) return NotFound();
        assetSubItem.is_active = false;
        assetSubItem.updated_at = DateTime.UtcNow;

        unit.Repository<AssetSubItem>().Update(assetSubItem);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset sub item");
    }

    [HttpPost("{id}/dispose")]
    public async Task<IActionResult> DisposeAssetSubItem(int id, AssetSubItemDisposalDto dto)
    {
        await unit.BeginTransactionAsync();
        try
        {
            var assetSubItem = await unit.Repository<AssetSubItem>().GetByIdAsync(id);
            if (assetSubItem == null) return NotFound("Asset sub item not found");

            if (assetSubItem.status == "จำหน่ายแล้ว")
                return BadRequest("This asset sub item has already been disposed");

            // Update sub-item status
            assetSubItem.status = "จำหน่ายแล้ว";
            assetSubItem.updated_at = DateTime.UtcNow;
            unit.Repository<AssetSubItem>().Update(assetSubItem);

            // Add disposal transaction record
            var disposal = mapper.Map<AssetSubItemDisposal>(dto);
            disposal.asset_sub_item_id = id;
            disposal.quantity_disposed = assetSubItem.quantity; // default to entire quantity
            disposal.is_active = true;
            disposal.created_at = DateTime.UtcNow;

            unit.Repository<AssetSubItemDisposal>().Add(disposal);

            if (await unit.Complete())
            {
                await unit.CommitTransactionAsync();
                return Ok(new { message = "Asset sub item disposed successfully" });
            }

            await unit.RollbackTransactionAsync();
            return BadRequest("Problem disposing asset sub item");
        }
        catch (Exception ex)
        {
            await unit.RollbackTransactionAsync();
            return StatusCode(500, $"An error occurred during disposal transaction: {ex.Message}");
        }
    }

    [HttpGet("{id}/disposal")]
    public async Task<ActionResult<AssetSubItemDisposalDto>> GetAssetSubItemDisposal(int id)
    {
        var spec = new Core.Specifications.BaseSpecification<AssetSubItemDisposal>(x => x.asset_sub_item_id == id);
        var disposal = await unit.Repository<AssetSubItemDisposal>().GetEntityWithSpec(spec);

        if (disposal == null) return NotFound("Disposal record not found for this sub item");

        return mapper.Map<AssetSubItemDisposalDto>(disposal);
    }
}
