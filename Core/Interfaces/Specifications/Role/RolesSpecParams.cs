using Core.Specifications;

namespace Core.Interfaces.Specifications.Roles;

public class RolesSpecParams : PagingParams
{
    private string? _search;

    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }

    public string? Sort { get; set; }
}