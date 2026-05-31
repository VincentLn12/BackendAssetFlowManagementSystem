using Core.Entities;
using Core.Interfaces.Specification.Expense_types;
using Core.Specifications;

public class Expense_typesSpecification : BaseSpecification<Expense_types>
{
    public Expense_typesSpecification(Expense_typesSpecParams specParams)
  : base(x => x.is_active  && 
  (
      string.IsNullOrEmpty(specParams.Search) ||
      x.expense_type_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.expense_type_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.expense_type_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.expense_type_id);
                break;

            default:
                AddOrderBy(x => x.expense_type_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

