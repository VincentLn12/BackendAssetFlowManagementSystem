using Core.Entities;
using Core.Interfaces.Specifications.AssetWithdrawal;
using Core.Specifications;

public class AssetWithdrawalSpecification : BaseSpecification<AssetWithdrawal>
{
    public AssetWithdrawalSpecification(
        AssetWithdrawalSpecParams specParams,
        int procurementRecordId
    )
    : base(x =>
        x.is_active &&
        x.procurement_record_id == procurementRecordId &&
        (
            string.IsNullOrEmpty(specParams.Search) ||
            x.Staff!.first_name.ToLower().Contains(specParams.Search.ToLower()) ||
            x.withdrawal_document_no.ToLower().Contains(specParams.Search.ToLower()) ||
            x.storage_location.ToLower().Contains(specParams.Search.ToLower())
        )
    )
    {
        AddInclude(x => x.Staff!);
        AddInclude(x => x.Staff!.Prefixes!);
        AddInclude(x => x.ProcurementRecord!);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.Staff!.first_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.Staff!.first_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.procurement_withdrawal_id);
                break;

            default:
                AddOrderBy(x => x.procurement_withdrawal_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }

    public AssetWithdrawalSpecification(AssetWithdrawalSpecParams specParams)
    : base(x =>
        x.is_active &&
        (
            string.IsNullOrEmpty(specParams.Search) ||
            x.Staff!.first_name.ToLower().Contains(specParams.Search.ToLower()) ||
            x.withdrawal_document_no.ToLower().Contains(specParams.Search.ToLower()) ||
            x.storage_location.ToLower().Contains(specParams.Search.ToLower())
        )
    )
    {
        AddInclude(x => x.Staff!);
        AddInclude(x => x.Staff!.Prefixes!);
        AddInclude(x => x.ProcurementRecord!);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.Staff!.first_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.Staff!.first_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.procurement_withdrawal_id);
                break;

            default:
                AddOrderBy(x => x.procurement_withdrawal_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}