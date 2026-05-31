using Core.Entities;
using Core.Interfaces.Specifications.Positions;
using Core.Specifications;

public class PositionsSpecification : BaseSpecification<Positions>
{
    public PositionsSpecification(PositionsSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.position_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.position_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.position_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.position_id);
                break;

            default:
                AddOrderBy(x => x.position_id);
                break;
        }


        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

