using Core.Specifications;

namespace Core.Interfaces.Specifications.AssetSubItem
{
    public class AssetSubItemSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string? _search;

        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }
        public int? Asset_id { get; set; }

    }
}
