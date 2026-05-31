using Core.Specifications;

namespace Core.Interfaces.Specifications.Procurement_records
{
    public class Procurement_recordsSpecParams : PagingParams
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
