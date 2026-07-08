namespace APi.DTOs
{
    public class ProcurementAssetFullCreateDto
    {
        public ProcurementRecordCreateDto procurement_record { get; set; } = new();

        public List<AssetItemWithSubItemsCreateDto> asset_items { get; set; } = new();
    }

    public class AssetItemWithSubItemsCreateDto
    {
        public AssetItemCreateDto asset_item { get; set; } = new();

        public List<AssetSubItemCreateDto> asset_sub_items { get; set; } = new();
    }
}
