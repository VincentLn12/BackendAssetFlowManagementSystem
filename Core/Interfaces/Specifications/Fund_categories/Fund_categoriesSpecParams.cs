using Core.Specifications;

namespace Core.Interfaces.Specifications.Fund_categories
{
    public class Fund_categoriesSpecParams : PagingParams
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
