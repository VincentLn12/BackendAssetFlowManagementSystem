using Core.Interfaces.Specifications.AssetUsageType;

namespace API.Controllers;

public class AssetUsageTypeController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetUsageType>>> GetAssetUsageTypes([FromQuery] AssetUsageTypeSpecParams  assetUsageTypeSpec)
    {
        var spec = new AssetUsageTypeSpecification(assetUsageTypeSpec);

        return await CreatePagedResult(unit.Repository<AssetUsageType>(), spec,
            assetUsageTypeSpec.PageIndex, assetUsageTypeSpec.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AssetUsageType>> GetAssetUsageType(int id)
    {
        var assetUsageType = await unit.Repository<AssetUsageType>().GetByIdAsync(id);

        if (assetUsageType == null) return NotFound();
        return assetUsageType;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AssetUsageType>> CreateAssetUsageType(AssetUsageType assetUsageType)
    {
        assetUsageType.is_active = true;
        assetUsageType.updated_at = null;
        unit.Repository<AssetUsageType>().Add(assetUsageType);
        if (await unit.Complete())
        {
            return CreatedAtAction("GetAssetUsageType", new { id = assetUsageType.usage_type_id }, assetUsageType);
        }
        ;

        return BadRequest("Problem creating asset usage type");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetUsageType(int id, AssetUsageType assetUsageType)
    {
        if (id != assetUsageType.usage_type_id)
            return BadRequest("Cannot update this asset usage type");

        var existingAssetUsageType = await unit.Repository<AssetUsageType>().GetByIdAsync(id);

        if (existingAssetUsageType == null)
            return NotFound("Asset usage type not found");

        existingAssetUsageType.usage_type_name = assetUsageType.usage_type_name;
        existingAssetUsageType.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset usage type");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetUsageType(int id)
    {
        var assetUsageType = await unit.Repository<AssetUsageType>().GetByIdAsync(id);

        if (assetUsageType == null) return NotFound();
        assetUsageType.is_active = false;
        assetUsageType.updated_at = DateTime.UtcNow;
        unit.Repository<AssetUsageType>().Update(assetUsageType);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset usage type");
    } 
}
