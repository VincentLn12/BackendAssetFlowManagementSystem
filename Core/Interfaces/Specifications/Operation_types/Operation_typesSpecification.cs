using Core.Entities;
using Core.Interfaces.Specifications.Operation_types;
using Core.Specifications;

public class Operation_typesSpecification : BaseSpecification<Operation_types>
{
    public Operation_typesSpecification(Operation_typesSpecParams specParams)
  : base(x => x.is_active  &&    
      (string.IsNullOrEmpty(specParams.Search) ||
      x.operation_type_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.operation_type_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.operation_type_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.operation_type_id);
                break;

            default:
                AddOrderBy(x => x.operation_type_id);
                break;
        }
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

