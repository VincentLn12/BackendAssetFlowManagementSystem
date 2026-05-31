using Core.Specifications;

namespace Core.Interfaces.Specifications.Budget_sources
{
    public class Budget_sourcesSpecParams : PagingParams
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
