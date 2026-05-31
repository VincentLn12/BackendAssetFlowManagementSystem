using Core.Interfaces;
using Core.Interfaces.Specifications.Prefixes;

namespace API.Controllers;

public class PrefixesController(IUnitOfWork unit) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Prefixes>>> GetPrefixes([FromQuery] PrefixesSpecParams prefixesParams)
    {
        var spec = new PrefixesSpecification(prefixesParams);

        return await CreatePagedResult(unit.Repository<Prefixes>(), spec,
            prefixesParams.PageIndex, prefixesParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Prefixes>> GetPrefix(int id)
    {
        var prefix = await unit.Repository<Prefixes>().GetByIdAsync(id);

        if (prefix == null) return NotFound();
        return prefix;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Prefixes>> CreatePrefix(Prefixes prefix      )
    {
        prefix.updated_at = null;
        unit.Repository<Prefixes>().Add(prefix);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetPrefix", new { id = prefix.prefix_id }, prefix);
        }
        ;

        return BadRequest("Problem creating prefix");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePrefix(int id, Prefixes prefix)
    {
        if (id != prefix.prefix_id)
            return BadRequest("Cannot update this prefix");

        var existingPrefix = await unit.Repository<Prefixes>().GetByIdAsync(id);

        if (existingPrefix == null)
            return NotFound("Prefix not found");

        existingPrefix.prefix_name = prefix.prefix_name;
        existingPrefix.prefix_short_name = prefix.prefix_short_name;
        existingPrefix.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the prefix");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrefix(int id)
    {
        var prefix = await unit.Repository<Prefixes>().GetByIdAsync(id);

        if (prefix == null) return NotFound();

        prefix.is_active = false;
        prefix.updated_at = DateTime.UtcNow;

        unit.Repository<Prefixes>().Update(prefix);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting prefix");
    }

}
