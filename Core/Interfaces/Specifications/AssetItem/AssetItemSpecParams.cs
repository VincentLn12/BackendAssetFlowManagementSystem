using Core.Specifications;

namespace Core.Interfaces.Specifications.AssetItem
{
    public class AssetItemSpecParams : PagingParams
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
