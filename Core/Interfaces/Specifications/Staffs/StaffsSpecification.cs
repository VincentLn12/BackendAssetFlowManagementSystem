using Core.Entities;
using Core.Interfaces.Specifications.Staffs;
using Core.Specifications;

public class StaffsSpecification : BaseSpecification<Staffs>
{
    public StaffsSpecification(StaffsSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.first_name.ToLower().Contains(specParams.Search) ||
                x.last_name.ToLower().Contains(specParams.Search)
            ))
    {
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.first_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.first_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.staff_id);
                break;

            default:
                AddOrderBy(x => x.staff_id);
                break;
        }

        AddInclude(x => x.Departments);
        AddInclude(x => x.Positions);
        AddInclude(x => x.Prefixes);

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}