using Core.Entities;
using Core.Interfaces.Specifications.HireDetail;
using Core.Specifications;

public class HireDetailSpecification : BaseSpecification<HireDetail>
{
    public HireDetailSpecification(HireDetailSpecParams specParams)
        : base(x => x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.hire_name.ToLower().Contains(specParams.Search.ToLower())
            ) &&
            (
                !specParams.ProcurementRecordId.HasValue ||
                x.procurement_record_id == specParams.ProcurementRecordId.Value
            )
        )
    {
        AddInclude(x => x.procurement_record);
        AddInclude(x => x.unit);


        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.item_no);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.item_no);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.created_at);
                break;

            default:
                AddOrderBy(x => x.item_no);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}