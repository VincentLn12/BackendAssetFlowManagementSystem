using Core.Interfaces;
using Core.Interfaces.Specifications;
namespace API.Controllers;

public class DepartmentsController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Departments>>> GetDepartments([FromQuery] DepartmentsSpecParams departmentsParams)
    {
        var spec = new DepartmentsSpecification(departmentsParams);

        return await CreatePagedResult(unit.Repository<Departments>(), spec,
            departmentsParams.PageIndex, departmentsParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Departments>> GetDepartment(int id)
    {
        var department = await unit.Repository<Departments>().GetByIdAsync(id);

        if (department == null) return NotFound();
        return department;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Departments>> CreateDepartment(Departments department)
    {
        department.updated_at = null;
        unit.Repository<Departments>().Add(department);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetDepartment", new { id = department.department_id }, department);
        }
        ;

        return BadRequest("Problem creating department");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, Departments department)
    {
        if (id != department.department_id)
            return BadRequest("Cannot update this department");

        var existingDepartment = await unit.Repository<Departments>().GetByIdAsync(id);

        if (existingDepartment == null)
            return NotFound("Department not found");

        existingDepartment.department_name = department.department_name;
        existingDepartment.is_active = department.is_active;
        existingDepartment.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the department");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await unit.Repository<Departments>().GetByIdAsync(id);

        if (department == null) return NotFound();

        department.is_active = false;
        department.updated_at = DateTime.UtcNow;

        unit.Repository<Departments>().Update(department);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting department");
    }

    private bool DepartmentExists(int id)
    {
        return unit.Repository<Departments>().GetByIdAsync(id).Result != null;
    }
}
