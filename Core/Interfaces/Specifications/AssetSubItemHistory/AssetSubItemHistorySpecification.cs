using API.Entities;
using Core.Interfaces.Specifications.AssetSubItemHistory;
using Core.Specifications;

public class AssetSubItemHistorySpecification : BaseSpecification<AssetSubItemHistory>
{
    public AssetSubItemHistorySpecification(AssetSubItemHistorySpecParams specParams)
        : base(x => x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.history_type.ToLower().Contains(specParams.Search.ToLower())
            ) &&
            (
                specParams.procurement_withdrawal_id == 0 ||
                x.procurement_withdrawal_id == specParams.procurement_withdrawal_id
            )
        )
    {
        AddInclude(x => x.AssetWithdrawal!);
        AddInclude(x => x.AssetUsageType!);     
        AddInclude(x => x.Staff!);
        AddInclude(x => x.Staff!.Prefixes!);


        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.history_type);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.history_type);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.sub_item_history_id);
                break;

            default:
                AddOrderBy(x => x.sub_item_history_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}