
using Core.Interfaces.Specifications.MaterialUnit;
using Core.Specifications;

public class MaterialUnitSpecification : BaseSpecification<MaterialUnit>
{
    public MaterialUnitSpecification(MaterialUnitSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.unit_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.unit_name);
                break;
            case "nameDesc":
                AddOrderByDescending(x => x.unit_name);
                break;
            case "idDesc":
                AddOrderByDescending(x => x.unit_id);
                break;
            default:
                AddOrderBy(x => x.unit_id);
                break;
        }

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

