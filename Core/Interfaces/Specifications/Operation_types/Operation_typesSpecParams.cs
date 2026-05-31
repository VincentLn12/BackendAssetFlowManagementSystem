using Core.Specifications;

namespace Core.Interfaces.Specifications.Operation_types
{
    public class Operation_typesSpecParams : PagingParams
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
