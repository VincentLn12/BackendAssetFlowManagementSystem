using Core.Entities;
using Core.Interfaces.Specifications.Fund_categories;
using Core.Specifications;

public class Fund_categoriesSpecification : BaseSpecification<Fund_categories>
{
    public Fund_categoriesSpecification(Fund_categoriesSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.fund_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.fund_name);
                break;
            case "nameDesc":
                AddOrderByDescending(x => x.fund_name);
                break;
            case "idDesc":
                AddOrderByDescending(x => x.fund_category_id);
                break;
            default:
                AddOrderBy(x => x.fund_category_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

