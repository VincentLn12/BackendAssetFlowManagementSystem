
using Core.Interfaces.Specifications.HireDetail;

namespace API.Controllers;

public class HiredetailsController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Projects>>> GetHiredetails([FromQuery] HireDetailSpecParams hireDetailParams)
    {
        var spec = new HireDetailSpecification(hireDetailParams);

        var projects = await unit.Repository<HireDetail>().ListAsync(spec);
         
        var countSpec = new HireDetailForCountSpecification(hireDetailParams);
        var totalItems = await unit.Repository<HireDetail>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<HireDetailDto>>(projects);

        return Ok(new Pagination<HireDetailDto>(
            hireDetailParams.PageIndex,
            hireDetailParams.PageSize,
            totalItems,
            data
        ));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<HireDetail>> GetHiredetails(int id)
    {
        var project = await unit.Repository<HireDetail>().GetByIdAsync(id);

        if (project == null) return NotFound();
        return project;
    }

    [HttpGet("by-procurement/{procurementRecordId}")]
    public async Task<ActionResult<IReadOnlyList<HireDetailDto>>> GetHireDetailsByProcurementRecordId(int procurementRecordId)
    {
        var spec = new HireDetailSpecification(new HireDetailSpecParams
        {
            ProcurementRecordId = procurementRecordId
        });

        var hireDetails = await unit.Repository<HireDetail>().ListAsync(spec);

        var data = mapper.Map<IReadOnlyList<HireDetailDto>>(hireDetails);

        return Ok(data);
    }

    [HttpPost("by-procurement/{procurementRecordId}")]
    public async Task<ActionResult<HireDetailDto>> CreateHireDetail(
    int procurementRecordId,
    HireDetailDto dto)
    {
        var hireDetail = mapper.Map<HireDetail>(dto);

        hireDetail.procurement_record_id = procurementRecordId;
        hireDetail.is_active = true;
        hireDetail.created_at = DateTime.Now;
        hireDetail.updated_at = DateTime.Now;

        unit.Repository<HireDetail>().Add(hireDetail);

        if (await unit.Complete())
        {
            var result = mapper.Map<HireDetailDto>(hireDetail);

            return Ok(result);
        }

        return BadRequest("Problem creating hire detail");
    }

    [HttpPost]
    public async Task<ActionResult<HireDetailDto>> GetHiredetails(
     HireDetailDto dto)
    {
        var hireDetail = mapper.Map<HireDetail>(dto);

        hireDetail.is_active = true;

        unit.Repository<HireDetail>().Add(hireDetail);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetHiredetails),
                new { id = hireDetail.hire_detail_id },
                hireDetail
            );
        }

        return BadRequest("Problem creating hire detail");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHiredetails(int id, HireDetailDto dto)
    {
        if (id != dto.hire_detail_id)
            return BadRequest("Cannot update this hire detail");

        var existingHireDetail = await unit.Repository<HireDetail>().GetByIdAsync(id);

        if (existingHireDetail == null)
            return NotFound("Hire detail not found");

        mapper.Map(dto, existingHireDetail);

        existingHireDetail.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the hire detail");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHiredetails(int id)
    {
        var hireDetail = await unit.Repository<HireDetail>().GetByIdAsync(id);

        if (hireDetail == null) return NotFound();
        hireDetail.is_active = false;
        hireDetail.updated_at = DateTime.UtcNow;
        unit.Repository<HireDetail>().Update(hireDetail);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting hire detail");
    }

}
