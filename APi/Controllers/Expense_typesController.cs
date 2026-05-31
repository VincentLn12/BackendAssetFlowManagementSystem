using Core.Interfaces;
using Core.Interfaces.Specification.Expense_types;

namespace API.Controllers;

public class Expense_typesController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Expense_types>>> GetExpenseTypes([FromQuery] Expense_typesSpecParams expenseTypesParams)
    {
        var spec = new Expense_typesSpecification(expenseTypesParams);

        return await CreatePagedResult(unit.Repository<Expense_types>(), spec,
            expenseTypesParams.PageIndex, expenseTypesParams.PageSize);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Expense_types>> GetExpenseType(int id)
    {
        var expenseType = await unit.Repository<Expense_types>().GetByIdAsync(id);

        if (expenseType == null) return NotFound();
        return expenseType;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Expense_types>> CreateExpenseType(Expense_types expenseType)
    {

        expenseType.is_active = true;
        expenseType.updated_at = null;
        unit.Repository<Expense_types>().Add(expenseType);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetExpenseType", new { id = expenseType.expense_type_id }, expenseType);
        }
        ;

        return BadRequest("Problem creating expense type");
    }


    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpenseType(int id, Expense_types expenseType)
    {
        if (id != expenseType.expense_type_id)
            return BadRequest("Cannot update this expense type");

        var existingExpenseType = await unit.Repository<Expense_types>().GetByIdAsync(id);

        if (existingExpenseType == null)
            return NotFound("Expense type not found");

        existingExpenseType.expense_type_name = expenseType.expense_type_name;
        existingExpenseType.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the expensetype");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpenseType(int id)
    {
        var expenseType = await unit.Repository<Expense_types>().GetByIdAsync(id);

        if (expenseType == null) return NotFound();
        expenseType.is_active = false;
        expenseType.updated_at = DateTime.UtcNow;   
        unit.Repository<Expense_types>().Update(expenseType);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting expense type");
    }

    private bool ExpenseTypeExists(int id)
    {
        return unit.Repository<Expense_types>().GetByIdAsync(id).Result != null;
    }
}
