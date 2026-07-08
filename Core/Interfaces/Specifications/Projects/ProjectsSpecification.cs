using Core.Entities;
using Core.Interfaces.Specifications.Projects;
using Core.Specifications;

public class ProjectsSpecification : BaseSpecification<Projects>
{
    public ProjectsSpecification(ProjectsSpecParams specParams)
        : base(x =>
            x.is_active &&

            // กรองตามปีงบประมาณ
            (!specParams.FiscalYearId.HasValue ||
             x.fiscal_year_id == specParams.FiscalYearId.Value) &&

            // ค้นหา
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.project_name.ToLower().Contains(specParams.Search.ToLower()) ||
                x.project_code.ToLower().Contains(specParams.Search.ToLower())  
            )
        )
    {
        AddInclude(x => x.fiscal_year!);
        AddInclude(x => x.staff!);
        AddInclude(x => x.staff!.Prefixes);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.project_code);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.project_code);
                break;

            case "oldest":
                AddOrderBy(x => x.created_at);
                break;

            case "latest":
                AddOrderByDescending(x => x.created_at);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.project_id);
                break;

            default:
                AddOrderByDescending(x => x.created_at);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}