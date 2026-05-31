
using Core.Interfaces.Specifications.Projects;


namespace API.Controllers;

public class ProjectsController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Projects>>> GetProjects([FromQuery] ProjectsSpecParams projectsParams)
    {
        var spec = new ProjectsSpecification(projectsParams);

        var projects = await unit.Repository<Projects>().ListAsync(spec);
         
        var countSpec = new ProjectsCountSpecification(projectsParams);

        var totalItems = await unit.Repository<Projects>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<ProjectDto>>(projects);

        return Ok(new Pagination<ProjectDto>(
            projectsParams.PageIndex,
            projectsParams.PageSize,
            totalItems,
            data
        ));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Projects>> GetProject(int id)
    {
        var project = await unit.Repository<Projects>().GetByIdAsync(id);

        if (project == null) return NotFound();
        return project;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(
     ProjectAddUpdateDto dto)
    {
        var project = mapper.Map<Projects>(dto);

        unit.Repository<Projects>().Add(project);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetProject),
                new { id = project.project_id },
                project
            );
        }

        return BadRequest("Problem creating project");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectAddUpdateDto dto)
    {
        if (id != dto.project_id)
            return BadRequest("Cannot update this project");

        var existingProject = await unit.Repository<Projects>().GetByIdAsync(id);

        if (existingProject == null)
            return NotFound("Project not found");

        mapper.Map(dto, existingProject);

        existingProject.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the project");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await unit.Repository<Projects>().GetByIdAsync(id);

        if (project == null) return NotFound();
        project.is_active = false;
        project.updated_at = DateTime.UtcNow;

        unit.Repository<Projects>().Update(project);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting fund category");
    }

}
