
using Core.Interfaces.Specifications.AssetItem;
using Core.Interfaces.Specifications.AssetWithdrawal;


namespace API.Controllers;

public class AssetWithdrawalController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AssetWithdrawal>>> GetAssetWithdrawals([FromQuery] AssetWithdrawalSpecParams assetWithdrawalParams)
    {
        var spec = new AssetWithdrawalSpecification(assetWithdrawalParams);

        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAsync(spec);
         
        var countSpec = new AssetWithdrawalSpecification(assetWithdrawalParams);    
        var totalItems = await unit.Repository<AssetWithdrawal>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<AssetWithdrawalDto>>(assetWithdrawals);

        return Ok(new Pagination<AssetWithdrawalDto>(
            assetWithdrawalParams.PageIndex,
            assetWithdrawalParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("by-procurement/{procurementRecordId}")]
    public async Task<ActionResult<Pagination<AssetWithdrawalDto>>> GetAssetWithdrawalsByProcurementRecordId(
      int procurementRecordId,
      [FromQuery] AssetWithdrawalSpecParams assetWithdrawalParams)
    {
        var spec = new AssetWithdrawalSpecification(assetWithdrawalParams, procurementRecordId);

        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAsync(spec);

        var countSpec = new AssetWithdrawalSpecification(assetWithdrawalParams, procurementRecordId);

        var totalItems = await unit.Repository<AssetWithdrawal>().CountAsync(countSpec);

        var data = mapper.Map<List<AssetWithdrawalDto>>(assetWithdrawals);

        return Ok(new Pagination<AssetWithdrawalDto>(
            assetWithdrawalParams.PageIndex,
            assetWithdrawalParams.PageSize,
            totalItems,
            data
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetWithdrawal>> GetAssetWithdrawal(int id)
    {
        var assetWithdrawal = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

        if (assetWithdrawal == null) return NotFound();
        return assetWithdrawal;
    }


    [HttpPost]
    public async Task<ActionResult<AssetWithdrawalCreateDto>> CreateAssetWithdrawal(
     AssetWithdrawalCreateDto dto)
    {
        var withdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();

        var nextNo = withdrawals
            .Where(x => x.procurement_record_id == dto.procurement_record_id && x.is_active)
            .Count() + 1;

        var assetWithdrawal = mapper.Map<AssetWithdrawal>(dto);

        assetWithdrawal.withdrawal_document_no = nextNo.ToString();
        assetWithdrawal.is_active = true;

        unit.Repository<AssetWithdrawal>().Add(assetWithdrawal);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetAssetWithdrawal),
                new { id = assetWithdrawal.procurement_withdrawal_id },
                assetWithdrawal
            );
        }

        return BadRequest("Problem creating asset withdrawal");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetWithdrawal(int id, AssetWithdrawalCreateDto dto)
    {
        if (id != dto.procurement_withdrawal_id)
            return BadRequest("Cannot update this asset withdrawal");

        var existingAssetWithdrawal = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

        if (existingAssetWithdrawal == null)
            return NotFound("Asset withdrawal not found");

        mapper.Map(dto, existingAssetWithdrawal);

        existingAssetWithdrawal.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset withdrawal");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetWithdrawal(int id)
    {
        var assetWithdrawal = await unit.Repository<AssetWithdrawal>().GetByIdAsync(id);

        if (assetWithdrawal == null) return NotFound();
        assetWithdrawal.is_active = false;
        assetWithdrawal.updated_at = DateTime.UtcNow;

        unit.Repository<AssetWithdrawal>().Update(assetWithdrawal);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting asset item");
    }

}
