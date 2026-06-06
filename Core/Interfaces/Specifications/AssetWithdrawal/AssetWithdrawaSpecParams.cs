using Core.Specifications;

namespace Core.Interfaces.Specifications.AssetWithdrawal
{
    public class AssetWithdrawalSpecParams : PagingParams
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
