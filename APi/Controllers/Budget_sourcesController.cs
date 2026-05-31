using Core.Interfaces;
using Core.Interfaces.Specifications.Budget_sources;
using Microsoft.OpenApi.Models;

namespace API.Controllers;

public class Budget_sourcesController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Budget_sources>>> GetBudget_sources([FromQuery] Budget_sourcesSpecParams budget_sourcesParams)
    {
        var spec = new Budget_sourcesSpecification(budget_sourcesParams);

        return await CreatePagedResult(unit.Repository<Budget_sources>(), spec,
            budget_sourcesParams.PageIndex, budget_sourcesParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Budget_sources>> GetBudget_source    (int id)
    {
        var budget_source = await unit.Repository<Budget_sources>().GetByIdAsync(id);

        if (budget_source == null) return NotFound();
        return budget_source;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Budget_sources>> CreateBudget_source(Budget_sources budget_source)
    {
        budget_source.is_active = true;
        budget_source.updated_at = null;
        unit.Repository<Budget_sources>().Add(budget_source);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetBudget_source", new { id = budget_source.budget_source_id }, budget_source);
        }
        ;

        return BadRequest("Problem creating budget source");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget_source(int id, Budget_sources budget_source)
    {
        if (id != budget_source.budget_source_id)
            return BadRequest("Cannot update this budget source");

        var existingBudgetSource = await unit.Repository<Budget_sources>().GetByIdAsync(id);

        if (existingBudgetSource == null)
            return NotFound("Budget source not found");

        existingBudgetSource.budget_source_name = budget_source.budget_source_name;
        existingBudgetSource.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the budget source");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget_source(int id)
    {
        var budget_source = await unit.Repository<Budget_sources>().GetByIdAsync(id);

        if (budget_source == null) return NotFound();
        budget_source.is_active = false;
        budget_source.updated_at = DateTime.UtcNow;

        unit.Repository<Budget_sources>().Update(budget_source);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting budget source");
    }

    private bool Budget_sourceExists(int id)
    {
        return unit.Repository<Budget_sources>().GetByIdAsync(id).Result != null;
    }
}
