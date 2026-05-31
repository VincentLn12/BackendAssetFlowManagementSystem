using Core.Interfaces.Specifications.AssetCategories;

namespace API.Controllers;

public class AssetCategoriesController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetCategory>>> GetAssetCategories([FromQuery] AssetCategoriesSpecParams  assetCategoriesSpec)
    {
        var spec = new AssetCategoriesSpecification(assetCategoriesSpec);

        return await CreatePagedResult(unit.Repository<AssetCategory>(), spec,
            assetCategoriesSpec.PageIndex, assetCategoriesSpec.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AssetCategory>> GetCategories(int id)
    {
        var assetCategory = await unit.Repository<AssetCategory>().GetByIdAsync(id);

        if (assetCategory == null) return NotFound();
        return assetCategory;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AssetCategory>> CreateCategories(AssetCategory assetCategory)
    {
        assetCategory.is_active = true;
        assetCategory.updated_at = null;
        unit.Repository<AssetCategory>().Add(assetCategory);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetCategories", new { id = assetCategory.asset_category_id }, assetCategory);
        }
        ;

        return BadRequest("Problem creating asset category");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategories(int id, AssetCategory assetCategory)
    {
        if (id != assetCategory.asset_category_id)
            return BadRequest("Cannot update this asset category");

        var existingAssetCategory = await unit.Repository<AssetCategory>().GetByIdAsync(id);

        if (existingAssetCategory == null)
            return NotFound("Asset category not found");

        existingAssetCategory.category_name = assetCategory.category_name;
        existingAssetCategory.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset category");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategories(int id)
    {
        var assetCategory = await unit.Repository<AssetCategory>().GetByIdAsync(id);

        if (assetCategory == null) return NotFound();
        assetCategory.is_active = false;
        assetCategory.updated_at = DateTime.UtcNow;

        unit.Repository<AssetCategory>().Update(assetCategory);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting position");
    } 
}
