using Core.Entities;
using Core.Interfaces.Specifications.Prefixes;
using Core.Specifications;

public class PrefixesSpecification : BaseSpecification<Prefixes>
{
    public PrefixesSpecification(PrefixesSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.prefix_name.ToLower().Contains(specParams.Search) ||
                x.prefix_short_name.ToLower().Contains(specParams.Search)
            ))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.prefix_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.prefix_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.prefix_id);
                break;

            default:
                AddOrderBy(x => x.prefix_id);
                break;
        }

        // PAGING
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}