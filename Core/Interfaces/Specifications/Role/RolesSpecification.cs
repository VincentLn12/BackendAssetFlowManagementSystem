using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Specifications.Roles;

public class RolesSpecification
{
    public IQueryable<IdentityRole> Apply(
        IQueryable<IdentityRole> query,
        RolesSpecParams specParams)
    {
        if (!string.IsNullOrEmpty(specParams.Search))
        {
            query = query.Where(x =>
                x.Name!.ToLower().Contains(specParams.Search));
        }

        query = specParams.Sort switch
        {
            "nameDesc" => query.OrderByDescending(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };

        query = query.Skip(
                specParams.PageSize *
                (specParams.PageIndex - 1))
            .Take(specParams.PageSize);

        return query;
    }
}