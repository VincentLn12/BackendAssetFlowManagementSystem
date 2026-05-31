using Core.Entities;
using Core.Interfaces.Specifications.Projects;
using Core.Specifications;

public class ProjectsSpecification : BaseSpecification<Projects>
{
    public ProjectsSpecification(ProjectsSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.project_name.ToLower().Contains(specParams.Search) ||
                x.project_code.ToLower().Contains(specParams.Search)
            ))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.project_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.project_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.project_id);
                break;

            default:
                AddOrderBy(x => x.project_id);
                break;
        }
        //include 
        AddInclude(x => x.fiscal_year);
        AddInclude(x => x.staff);
        // PAGING
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}