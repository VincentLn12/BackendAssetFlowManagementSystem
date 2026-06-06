
using Core.Entities;
using Core.Interfaces.Specifications.MaterialItem;
using Core.Specifications;

public class MaterialItemSpecification : BaseSpecification<MaterialItem>
{
    public MaterialItemSpecification(MaterialItemSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.material_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.material_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.material_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.material_item_id);
                break;

            default:
                AddOrderBy(x => x.material_item_id);
                break;
        }

        AddInclude(x => x.Unit!);

        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

