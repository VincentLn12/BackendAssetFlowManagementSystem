using Core.Specifications;

namespace Core.Interfaces.Specifications.MaterialUnit
{
    public class MaterialUnitSpecParams : PagingParams
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
