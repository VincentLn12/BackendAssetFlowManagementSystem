using Core.Specifications;

namespace Core.Interfaces.Specifications.Prefixes
{
    public class PrefixesSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
    }
}
