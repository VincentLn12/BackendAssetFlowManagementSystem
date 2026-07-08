using Core.Entities;
using Core.Interfaces.Specifications.MaterialWithdrawal;
using Core.Specifications;

public class MaterialWithdrawalSpecification : BaseSpecification<MaterialWithdrawal>
{
    public MaterialWithdrawalSpecification(
        MaterialWithdrawalSpecParams specParams,
        int procurementRecordId
    )
    : base(x =>
        x.is_active &&
        x.procurement_record_id == procurementRecordId &&
        (
            string.IsNullOrEmpty(specParams.Search) ||
            x.staffs!.first_name.ToLower().Contains(specParams.Search.ToLower()) ||
            x.withdrawal_document_no.ToLower().Contains(specParams.Search.ToLower()) 
        )
    )
    {
        AddInclude(x => x.staffs!);
        AddInclude(x => x.staffs!.Prefixes!);
        AddInclude(x => x.ProcurementRecord!);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.staffs!.first_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.staffs!.first_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.material_withdrawal_id);
                break;

            default:
                AddOrderBy(x => x.material_withdrawal_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }

    public MaterialWithdrawalSpecification(MaterialWithdrawalSpecParams specParams)
    : base(x =>
        x.is_active &&
        (
            string.IsNullOrEmpty(specParams.Search) ||
            x.staffs!.first_name.ToLower().Contains(specParams.Search.ToLower()) ||
            x.withdrawal_document_no.ToLower().Contains(specParams.Search.ToLower()))
        
    )
    {
        AddInclude(x => x.staffs!);
        AddInclude(x => x.staffs!.Prefixes!);
        AddInclude(x => x.ProcurementRecord!);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.staffs!.first_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.staffs!.first_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.material_withdrawal_id);
                break;

            default:
                AddOrderBy(x => x.material_withdrawal_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}