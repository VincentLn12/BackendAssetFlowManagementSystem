using Core.Interfaces.Specifications.AcquisitionMethod;

namespace API.Controllers;

public class AcquisitionMethodController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AcquisitionMethod>>> GetAcquisitionMethod([FromQuery] AcquisitionMethodSpecParams acquisitionMethodSpec)
    {
        var spec = new AcquisitionMethodSpecification(acquisitionMethodSpec);

        return await CreatePagedResult(unit.Repository<AcquisitionMethod>(), spec,
            acquisitionMethodSpec.PageIndex, acquisitionMethodSpec.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AcquisitionMethod>> GetAcquisitionMethod(int id)
    {
        var acquisition = await unit.Repository<AcquisitionMethod>().GetByIdAsync(id);

        if (acquisition == null) return NotFound();
        return acquisition;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AcquisitionMethod>> GetAcquisitionMethod(AcquisitionMethod acquisition)
    {
        acquisition.is_active = true;
        acquisition.updated_at = null;
        unit.Repository<AcquisitionMethod>().Add(acquisition);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetAcquisitionMethod", new { id = acquisition.acquisition_method_id }, acquisition);
        }
        ;

        return BadRequest("Problem creating acquisition");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> GetAcquisitionMethod(int id, AcquisitionMethod acquisition)
    {
        if (id != acquisition.acquisition_method_id)
            return BadRequest("Cannot update this asset category");

        var existingacquisition = await unit.Repository<AcquisitionMethod>().GetByIdAsync(id);

        if (existingacquisition == null)
            return NotFound("Asset category not found");

        existingacquisition.acquisition_method_name = acquisition.acquisition_method_name;
        existingacquisition.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the asset category");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAcquisitionMethod(int id)
    {
        var acquisition = await unit.Repository<AcquisitionMethod>().GetByIdAsync(id);

        if (acquisition == null) return NotFound();
        acquisition.is_active = false;
        acquisition.updated_at = DateTime.UtcNow;

        unit.Repository<AcquisitionMethod>().Update(acquisition);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting position");
    } 
}
