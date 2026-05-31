using Core.Interfaces.Specifications.MaterialUnit;
namespace API.Controllers;

public class MaterialUnitController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaterialUnit>>> GetMaterialUnits([FromQuery] MaterialUnitSpecParams materialUnitParams)
    {
        var spec = new MaterialUnitSpecification(materialUnitParams);

        return await CreatePagedResult(unit.Repository<MaterialUnit>(), spec,
            materialUnitParams.PageIndex, materialUnitParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialUnit>> GetMaterialUnit(int id)
    {
        var materialUnit = await unit.Repository<MaterialUnit>().GetByIdAsync(id);

        if (materialUnit == null) return NotFound();
        return materialUnit    ;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<MaterialUnit>> CreateMaterialUnit(MaterialUnit materialUnit)
    {
        materialUnit.is_active = true;
        materialUnit.updated_at = null;
        unit.Repository<MaterialUnit>().Add(materialUnit);
        if (await unit.Complete())
        {
            return CreatedAtAction("GetMaterialUnit", new { id = materialUnit.unit_id }, materialUnit);
        }
        ;

        return BadRequest("Problem creating material unit");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialUnit(int id, MaterialUnit materialUnit)
    {
        if (id != materialUnit.unit_id)
            return BadRequest("Cannot update this material unit");

        var existingMaterialUnit = await unit.Repository<MaterialUnit>().GetByIdAsync(id);

        if (existingMaterialUnit == null)
            return NotFound("Material unit not found");

        existingMaterialUnit.unit_name = materialUnit.unit_name;
        existingMaterialUnit.updated_at = DateTime.Now;
        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the material unit");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialUnit(int id)
    {
        var materialUnit = await unit.Repository<MaterialUnit>().GetByIdAsync(id);

        if (materialUnit == null) return NotFound();
        materialUnit.is_active = false;
        materialUnit.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialUnit>().Update(materialUnit);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting material unit");
    }

}
