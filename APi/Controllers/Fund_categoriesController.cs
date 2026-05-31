using Core.Interfaces;
using Core.Interfaces.Specifications.Fund_categories;
using Microsoft.OpenApi.Models;

namespace API.Controllers;

public class Fund_categoriesController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Fund_categories>>> GetFund_categories([FromQuery] Fund_categoriesSpecParams fund_categoriesParams)
    {
        var spec = new Fund_categoriesSpecification(fund_categoriesParams);

        return await CreatePagedResult(unit.Repository<Fund_categories>(), spec,
            fund_categoriesParams.PageIndex, fund_categoriesParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Fund_categories>> GetFund_category(int id)
    {
        var fund_category = await unit.Repository<Fund_categories>().GetByIdAsync(id);

        if (fund_category == null) return NotFound();
        return fund_category    ;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Fund_categories>> CreateFund_category(Fund_categories fund_category)
    {
        fund_category.is_active = true;
        fund_category.updated_at = null;
        unit.Repository<Fund_categories>().Add(fund_category);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetFund_category", new { id = fund_category.fund_category_id }, fund_category);
        }
        ;

        return BadRequest("Problem creating fund category");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFund_category(int id, Fund_categories fund_category)
    {
        if (id != fund_category.fund_category_id)
            return BadRequest("Cannot update this fund category");

        var existingFundCategory = await unit.Repository<Fund_categories>().GetByIdAsync(id);

        if (existingFundCategory == null)
            return NotFound("Fund category not found");

        existingFundCategory.fund_name = fund_category.fund_name;
        existingFundCategory.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the fund category");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFund_category(int id)
    {
        var fund_category = await unit.Repository<Fund_categories>().GetByIdAsync(id);

        if (fund_category == null) return NotFound();
        fund_category.is_active = false;
        fund_category.updated_at = DateTime.UtcNow;

        unit.Repository<Fund_categories>().Update(fund_category);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting fund category");
    }

}
