using Core.Interfaces;
using Core.Interfaces.Specifications.Positions;

namespace API.Controllers;

public class PositionsController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Positions>>> GetPositions([FromQuery] PositionsSpecParams positionsParams)
    {
        var spec = new PositionsSpecification(positionsParams);

        return await CreatePagedResult(unit.Repository<Positions>(), spec,
            positionsParams.PageIndex, positionsParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Positions>> GetPosition(int id)
    {
        var position = await unit.Repository<Positions>().GetByIdAsync(id);

        if (position == null) return NotFound();
        return position;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Positions>> CreatePosition(Positions position      )
    {
        position.updated_at = null;
        unit.Repository<Positions>().Add(position);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetPosition", new { id = position.position_id }, position);
        }
        ;

        return BadRequest("Problem creating position");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePosition(int id, Positions position)
    {
        if (id != position.position_id)
            return BadRequest("Cannot update this position");

        var existingPosition = await unit.Repository<Positions>().GetByIdAsync(id);

        if (existingPosition == null)
            return NotFound("Position not found");

        existingPosition.position_name = position.position_name;
        existingPosition.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the prefix");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        var position = await unit.Repository<Positions>().GetByIdAsync(id);

        if (position == null) return NotFound();
        position.is_active = false;
        position.updated_at = DateTime.UtcNow;

        unit.Repository<Positions>().Update(position);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting position");
    } 
}
