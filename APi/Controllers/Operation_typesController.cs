using Core.Interfaces;
using Core.Interfaces.Specifications.Operation_types;

namespace API.Controllers;

public class Operation_typesController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Operation_types>>> GetOperationTypes([FromQuery] Operation_typesSpecParams operationTypesParams)
    {
        var spec = new Operation_typesSpecification(operationTypesParams);

        return await CreatePagedResult(unit.Repository<Operation_types>(), spec,
            operationTypesParams.PageIndex, operationTypesParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Operation_types>> GetOperationType(int id)
    {
        var operationType = await unit.Repository<Operation_types>().GetByIdAsync(id);

        if (operationType == null) return NotFound();
        return operationType;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Operation_types>> CreateOperationType(Operation_types operationType)
    {
        operationType.updated_at = null;
        unit.Repository<Operation_types>().Add(operationType);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetOperationType", new { id = operationType.operation_type_id }, operationType);
        }
        ;

        return BadRequest("Problem creating operation type");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOperationType(int id, Operation_types operationType)
    {
        if (id != operationType.operation_type_id)
            return BadRequest("Cannot update this operation type");

        var existingOperationType = await unit.Repository<Operation_types>().GetByIdAsync(id);

        if (existingOperationType == null)
            return NotFound("Operation type not found");

        existingOperationType.operation_type_name = operationType.operation_type_name;
        existingOperationType.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the operation type");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperationType(int id)
    {
        var operationType = await unit.Repository<Operation_types>().GetByIdAsync(id);

        if (operationType == null) return NotFound();
        operationType.is_active = false;
        operationType.updated_at = DateTime.UtcNow;

        unit.Repository<Operation_types>().Update(operationType);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting operation type");
    }

    private bool OperationTypeExists(int id)
    {
        return unit.Repository<Operation_types>().GetByIdAsync(id).Result != null;
    }
}
