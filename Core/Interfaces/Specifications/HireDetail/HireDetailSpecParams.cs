using Core.Specifications;

namespace Core.Interfaces.Specifications.HireDetail
{
    public class HireDetailSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
        public int? ProcurementRecordId { get; set; }

    }
}
