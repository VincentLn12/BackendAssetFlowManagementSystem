namespace APi.DTOs
{
    public class AssetSubItemDto
    {
        public int asset_sub_item_id { get; set; }
        public int asset_id { get; set; }
        public string asset_code_start { get; set; } = string.Empty;
        public string asset_code_end { get; set; } = string.Empty; public int? item_no { get; set; }
        public string sub_item_name { get; set; } = string.Empty;
        public int asset_category_id { get; set; }
        public string category_name { get; set; } = string.Empty;
        public int running_start_no { get; set; }
        public int running_end_no { get; set; }
        public int fiscal_asset_year { get; set; }
        public decimal quantity { get; set; }
        public string quantity_with_unit { get; set; } = string.Empty;
        public int unit_id { get; set; }   
        public string unit_name { get; set; } = string.Empty;
        public decimal? unit_price { get; set; }
        public decimal? total_price { get; set; }
        public int useful_life_year { get; set; }
    }
    public class AssetSubItemCreateDto
    {
        public int? asset_sub_item_id { get; set; }
        public int asset_id { get; set; }
        public int? item_no { get; set; }
        public string sub_item_name { get; set; } = string.Empty;
        public int asset_category_id { get; set; }
        public int running_start_no { get; set; }
        public int running_end_no { get; set; }
        public int fiscal_asset_year { get; set; }
        public decimal quantity { get; set; }
        public int unit_id { get; set; }   
        public decimal? unit_price { get; set; }
        public decimal? total_price { get; set; }
        public int useful_life_year { get; set; }
    }
}
