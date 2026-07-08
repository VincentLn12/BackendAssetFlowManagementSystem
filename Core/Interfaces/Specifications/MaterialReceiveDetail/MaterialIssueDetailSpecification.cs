using Core.Entities;
using Core.Interfaces.Specifications.MaterialReceiveDetail;
using Core.Specifications;

public class MaterialReceiveDetailSpecification : BaseSpecification<MaterialReceiveDetail>
{
    public MaterialReceiveDetailSpecification(MaterialReceiveDetailSpecParams specParams)
        : base(x =>
            x.is_active &&
            (!specParams.procurement_record_id.HasValue ||
             x.procurement_record_id == specParams.procurement_record_id.Value) &&
            (string.IsNullOrEmpty(specParams.Search) ||
             (x.operation_reason != null &&
              x.operation_reason.ToLower().Contains(specParams.Search.ToLower())) ||
             (x.MaterialItem != null &&
              x.MaterialItem.material_name.ToLower().Contains(specParams.Search.ToLower())))
        )
    {
        AddInclude(x => x.MaterialItem!);
        AddInclude(x => x.ProcurementRecord!);

        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.MaterialItem!.material_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.MaterialItem!.material_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.receive_detail_id);
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