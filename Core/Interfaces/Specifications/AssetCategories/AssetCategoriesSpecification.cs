
using Core.Interfaces.Specifications.AssetCategories;
using Core.Specifications;

public class AssetCategoriesSpecification : BaseSpecification<AssetCategory>
{
    public AssetCategoriesSpecification(AssetCategoriesSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.category_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.category_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.category_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.asset_category_id);
                break;

            default:
                AddOrderBy(x => x.asset_category_id);
                break;
        }


        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

