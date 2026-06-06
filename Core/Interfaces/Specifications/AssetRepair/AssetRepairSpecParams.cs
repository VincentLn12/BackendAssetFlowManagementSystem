using Core.Specifications;

namespace Core.Interfaces.Specifications.AssetRepair
{
    public class AssetRepairSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
        public int? asset_id { get; set; }
    }
}
