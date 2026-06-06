
using Core.Entities;
using Core.Interfaces.Specifications.AssetUsageType;
using Core.Specifications;

public class AssetUsageTypeSpecification : BaseSpecification<AssetUsageType>
{
    public AssetUsageTypeSpecification(AssetUsageTypeSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.usage_type_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.usage_type_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.usage_type_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.usage_type_id);
                break;

            default:
                AddOrderBy(x => x.usage_type_id);
                break;
        }


        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

