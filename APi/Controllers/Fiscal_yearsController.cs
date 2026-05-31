using Core.Interfaces;
using Core.Interfaces.Specifications.Fiscal_years;

namespace API.Controllers;

public class Fiscal_yearsController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Fiscal_years>>> GetFiscal_years([FromQuery] Fiscal_yearsSpecParams fiscal_yearsParams)
    {
        var spec = new Fiscal_yearsSpecification(fiscal_yearsParams );

        return await CreatePagedResult(unit.Repository<Fiscal_years>(), spec,
            fiscal_yearsParams.PageIndex, fiscal_yearsParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Fiscal_years>> GetFiscal_year(int id)
    {
        var fiscal_year = await unit.Repository<Fiscal_years>().GetByIdAsync(id);

        if (fiscal_year == null) return NotFound();
        return fiscal_year;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Fiscal_years>> CreateFiscal_year(Fiscal_years fiscal_year)
    {
        fiscal_year.updated_at = null;
        unit.Repository<Fiscal_years>().Add(fiscal_year);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetFiscal_year", new { id = fiscal_year.fiscal_year_id }, fiscal_year);
        }
        ;

        return BadRequest("Problem creating fiscal year");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFiscal_year(int id, Fiscal_years fiscal_year)
    {
        if (id != fiscal_year.fiscal_year_id)
            return BadRequest("Cannot update this fiscal year");

        var existingFiscalYear = await unit.Repository<Fiscal_years>().GetByIdAsync(id);

        if (existingFiscalYear == null)
            return NotFound("Fiscal year not found");

        existingFiscalYear.fiscal_year = fiscal_year.fiscal_year;
        existingFiscalYear.year_name = fiscal_year.year_name;
        existingFiscalYear.start_date = fiscal_year.start_date;
        existingFiscalYear.end_date = fiscal_year.end_date;
        existingFiscalYear .is_closed = fiscal_year.is_closed;

        existingFiscalYear.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the fiscal year");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFiscal_year(int id)
    {
        var fiscal_year = await unit.Repository<Fiscal_years>().GetByIdAsync(id);

        if (fiscal_year == null) return NotFound();

        fiscal_year.is_active = false;
        fiscal_year.updated_at = DateTime.UtcNow;
        unit.Repository<Fiscal_years>().Update(fiscal_year);


        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting fiscal year");
    }

}
