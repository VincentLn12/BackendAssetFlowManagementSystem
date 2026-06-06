using Core.Specifications;

namespace Core.Interfaces.Specifications.AssetSubItemHistory
{
    public class AssetSubItemHistorySpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
        public int procurement_withdrawal_id { get; set; }

    }
}
