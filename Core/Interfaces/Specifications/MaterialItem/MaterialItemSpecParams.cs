using Core.Specifications;

namespace Core.Interfaces.Specifications.MaterialItem
{
    public class MaterialItemSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }

        public int? FiscalYearId { get; set; }
    }
}
