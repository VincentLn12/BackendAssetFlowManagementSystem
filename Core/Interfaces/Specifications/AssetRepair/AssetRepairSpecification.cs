
using Core.Entities;
using Core.Interfaces.Specifications.AssetRepair;
using Core.Specifications;

public class AssetRepairSpecification : BaseSpecification<AssetRepair>
{
    public AssetRepairSpecification(AssetRepairSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.repair_shop_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.repair_shop_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.repair_shop_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.asset_repair_id);
                break;

            default:
                AddOrderBy(x => x.asset_repair_id);
                break;
        }

        AddInclude(x => x.Staff!);
        AddInclude(x => x.Staff!.Prefixes!);
        AddInclude(x => x.assetWithdrawal!);

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

