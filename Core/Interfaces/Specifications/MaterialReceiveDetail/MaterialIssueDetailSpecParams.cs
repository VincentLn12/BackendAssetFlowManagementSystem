using Core.Specifications;

namespace Core.Interfaces.Specifications.MaterialReceiveDetail
{
    public class MaterialReceiveDetailSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
        public int? procurement_record_id { get; set; }
    }
}
