
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

}
