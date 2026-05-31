using Core.Entities;
using Core.Interfaces.Specifications.Fiscal_years;
using Core.Specifications;

public class Fiscal_yearsSpecification : BaseSpecification<Fiscal_years>
{
    public Fiscal_yearsSpecification(Fiscal_yearsSpecParams specParams)
  : base(x => x.is_active == true &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.year_name.ToLower().Contains(specParams.Search.ToLower()))) 
    {      
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.year_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.year_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.fiscal_year_id);
                break;

            default:
                AddOrderBy(x => x.fiscal_year_id);
                break;
        }
        AddOrderByDescending(p => p.fiscal_year_id);
    }
}
    

