
using Core.Interfaces.Specifications.AssetItem;


namespace API.Controllers;

public class AssetItemController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetItem>>> GetAssetItems([FromQuery] AssetItemSpecParams assetItemParams)
    {
        var spec = new AssetItemSpecification(assetItemParams);

        var assetItems = await unit.Repository<AssetItem>().ListAsync(spec);
         
        var countSpec = new AssetItemForCountSpecification(assetItemParams);    
        var totalItems = await unit.Repository<AssetItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetItemDto>>(assetItems);

        return Ok(new Pagination<AssetItemDto>(
            assetItemParams.PageIndex,
            assetItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-procurement/{procurementRecordId}")]
    public async Task<ActionResult<Pagination<AssetItemDto>>> GetAssetItemsByProcurementRecordId(
      int procurementRecordId,
      [FromQuery] AssetItemSpecParams assetItemParams)
    {
        assetItemParams.ProcurementRecordId = procurementRecordId;

        var spec = new AssetItemSpecification(assetItemParams);

        var assetItems = await unit.Repository<AssetItem>().ListAsync(spec);

        var countSpec = new AssetItemForCountSpecification(assetItemParams);

        var totalItems = await unit.Repository<AssetItem>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetItemDto>>(assetItems);

        return Ok(new Pagination<AssetItemDto>(
            assetItemParams.PageIndex,
            assetItemParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetItem>> GetAssetItem(int id)
    {
        var assetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (assetItem == null) return NotFound();
        return assetItem;
    }

    [HttpPost]
    public async Task<ActionResult<AssetItemCreateDto>> CreateAssetItem(
         AssetItemCreateDto dto)
        {
        var assetItem = mapper.Map<AssetItem>(dto);

        assetItem.is_active = true;

        unit.Repository<AssetItem>().Add(assetItem);
        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetItem),
                new { id = assetItem.asset_id },
                assetItem
            );
        }

        return BadRequest("Problem creating project");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetItem(int id, AssetItemCreateDto dto)
    {
        if (id != dto.asset_id)
            return BadRequest("Cannot update this asset item");

        var existingAssetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (existingAssetItem == null)
            return NotFound("Asset item not found");

        mapper.Map(dto, existingAssetItem);

        existingAssetItem.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the project");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetItem(int id)
    {
        var assetItem = await unit.Repository<AssetItem>().GetByIdAsync(id);

        if (assetItem == null) return NotFound();
        assetItem.is_active = false;
        assetItem.updated_at = DateTime.UtcNow;

        unit.Repository<AssetItem>().Update(assetItem);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset item");
    }

}
