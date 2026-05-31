using Core.Entities;
using Core.Interfaces.Specifications.Budget_sources;
using Core.Specifications;

public class Budget_sourcesSpecification : BaseSpecification<Budget_sources>
{
    public Budget_sourcesSpecification(Budget_sourcesSpecParams specParams)
  : base(x => x.is_active &&
  ( 
      string.IsNullOrEmpty(specParams.Search) ||
      x.budget_source_name.ToLower().Contains(specParams.Search)))
    {
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.budget_source_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.budget_source_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.budget_source_id);
                break;

            default:
                AddOrderBy(x => x.budget_source_id);
                break;
        }
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

