using Core.Interfaces.Specifications.Staffs;

namespace API.Controllers;

public class StaffController(IUnitOfWork unit , IMapper mapper ) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<Pagination<StaffDto>>> GetStaffs(
     [FromQuery] StaffsSpecParams staffsParams)
    {
        var spec = new StaffsSpecification(staffsParams);

        var staffs = await unit.Repository<Staffs>().ListAsync(spec);

        var countSpec = new StaffsForCountSpecification(staffsParams);

        var totalItems = await unit.Repository<Staffs>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<StaffDto>>(staffs);

        return Ok(new Pagination<StaffDto>(
            staffsParams.PageIndex,
            staffsParams.PageSize,
            totalItems,
            data
        ));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Staffs>> GetStaff(int id)
    {
        var staffs = await unit.Repository<Staffs>().GetByIdAsync(id);

        if (staffs == null) return NotFound();
        return staffs;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Staffs>> CreateStaff(StaffCreateDto staff)
    {
        var newStaff = new Staffs
        {
            first_name = staff.first_name,
            last_name = staff.last_name,
            email = staff.email,
            phone = staff.phone,
            department_id = staff.department_id,
            position_id = staff.position_id,
            prefix_id = staff.prefix_id,
            is_active = true,
            updated_at = null
        };

        unit.Repository<Staffs>().Add(newStaff);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetStaff", new { id = staff.staff_id }, staff);
        }
        ;

        return BadRequest("Problem creating staff");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePrefix(int id, StaffCreateDto staffs)
    {
        if (id != staffs.staff_id)
            return BadRequest("Cannot update this staffs");

        var existingStaffs = await unit.Repository<Staffs>().GetByIdAsync(id);

        if (existingStaffs == null)
            return NotFound("Prefix not found");

        existingStaffs.first_name = staffs.first_name;
        existingStaffs.last_name = staffs.last_name;
        existingStaffs.email = staffs.email;
        existingStaffs.phone = staffs.phone;
        existingStaffs.department_id = staffs.department_id;
        existingStaffs.position_id = staffs.position_id;
        existingStaffs.prefix_id = staffs.prefix_id;

        existingStaffs.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the prefix");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        var staff = await unit.Repository<Staffs>().GetByIdAsync(id);

        if (staff == null) return NotFound();
        staff.is_active = false;
        staff.updated_at = DateTime.UtcNow;

        unit.Repository<Staffs>().Update(staff);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting staff");
    }

    private async Task<bool> StaffExists(int id)
    {
        return await unit.Repository<Staffs>().GetByIdAsync(id) != null;
    }
}
