using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces.Specifications;

public class DepartmentsSpecification : BaseSpecification<Departments>
{
    public DepartmentsSpecification(DepartmentsSpecParams specParams)
        : base(x => x.is_active == true &&
            (string.IsNullOrEmpty(specParams.Search) ||
            x.department_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.department_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.department_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.department_id);
                break;

            default:
                AddOrderBy(x => x.department_id);
                break;
        }

        // PAGING
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}