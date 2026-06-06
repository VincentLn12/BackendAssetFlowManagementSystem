
using Core.Interfaces.Specifications.AssetRepair;


namespace API.Controllers;

public class AssetRepairController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetRepairDto>>> GetAssetRepair([FromQuery] AssetRepairSpecParams assetRepairParams)
    {
        var spec = new AssetRepairSpecification(assetRepairParams);

        var assetRepairs = await unit.Repository<AssetRepair>().ListAsync(spec);
         
        var countSpec = new AssetRepairSpecification(assetRepairParams);    
        var totalItems = await unit.Repository<AssetRepair>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetRepairDto>>(assetRepairs);

        return Ok(new Pagination<AssetRepairDto>(
            assetRepairParams.PageIndex,
            assetRepairParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-asset/{asset_id:int}")]
    public async Task<ActionResult<Pagination<AssetRepairDto>>> GetAssetRepairsByProcurementRecordId(
      int asset_id,
      [FromQuery] AssetRepairSpecParams assetRepairParams)
    {
        assetRepairParams.asset_id = asset_id;

        var spec = new AssetRepairSpecification(assetRepairParams);
        var assetRepairs = await unit.Repository<AssetRepair>().ListAsync(spec);

        var countSpec = new AssetRepairSpecification(assetRepairParams);

        var totalItems = await unit.Repository<AssetRepair>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetRepairDto>>(assetRepairs);

        return Ok(new Pagination<AssetRepairDto>(
            assetRepairParams.PageIndex,
            assetRepairParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetRepairDto>> GetAssetRepair(int id)
    {
        var assetRepair = await unit.Repository<AssetRepair>().GetByIdAsync(id);

        if (assetRepair == null) return NotFound();
        return mapper.Map<AssetRepairDto>(assetRepair);
    }

    [HttpPost]
    public async Task<ActionResult<AssetRepairDto>> CreateAssetRepair(AssetRepairDto dto)
    {
        var repairs = await unit.Repository<AssetRepair>().ListAllAsync();

        var nextNo = repairs
            .Where(x => x.procurement_withdrawal_id == dto.procurement_withdrawal_id && x.is_active)
            .Count() + 1;

        var assetRepair = mapper.Map<AssetRepair>(dto);

        assetRepair.repair_document_no = nextNo.ToString();
        assetRepair.is_active = true;

        unit.Repository<AssetRepair>().Add(assetRepair);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetRepair),
                new { id = assetRepair.asset_repair_id },
                mapper.Map<AssetRepairDto>(assetRepair)
            );
        }

        return BadRequest("Problem creating asset repair");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetRepair(int id, AssetRepairDto dto)
    {
        if (id != dto.asset_repair_id)
            return BadRequest("Cannot update this asset repair");

        var existingAssetRepair = await unit.Repository<AssetRepair>().GetByIdAsync(id);

        if (existingAssetRepair == null)
            return NotFound("Asset repair not found");

        mapper.Map(dto, existingAssetRepair);

        existingAssetRepair.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset repair");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetRepair(int id)
    {
        var assetRepair = await unit.Repository<AssetRepair>().GetByIdAsync(id);

        if (assetRepair == null) return NotFound();
        assetRepair.is_active = false;
        assetRepair.updated_at = DateTime.UtcNow;

        unit.Repository<AssetRepair>().Update(assetRepair);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset repair");
    }

}
