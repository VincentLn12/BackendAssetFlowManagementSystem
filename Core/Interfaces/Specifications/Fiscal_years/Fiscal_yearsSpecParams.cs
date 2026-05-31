using Core.Specifications;

namespace Core.Interfaces.Specifications.Fiscal_years
{
    public class Fiscal_yearsSpecParams : PagingParams
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
