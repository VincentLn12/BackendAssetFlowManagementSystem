
using Core.Entities;
using Core.Interfaces.Specifications.MaterialIssueDetail;
using Core.Specifications;

public class MaterialIssueDetailSpecification : BaseSpecification<MaterialIssueDetail>
{
    public MaterialIssueDetailSpecification(MaterialIssueDetailSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.remark!.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.remark!);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.remark!);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.material_item_id);
                break;

            default:
                AddOrderBy(x => x.material_item_id);
                break;
        }

        AddInclude(x => x.Requester!);
        AddInclude(x => x.Requester!.Prefixes!);
        AddInclude(x => x.MaterialItem!);


        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

