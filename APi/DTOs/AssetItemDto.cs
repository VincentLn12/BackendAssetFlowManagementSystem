
namespace APi.DTOs
{
    public class AssetItemDto
    {
        public int asset_id { get; set; }
        public int? procurement_record_id { get; set; }
        public int? item_no { get; set; }
        public string asset_code_prefix { get; set; } = string.Empty;
        public string asset_name { get; set; } = string.Empty;
        //public decimal quantity { get; set; }
        //public decimal unit_price { get; set; }
        //public decimal total_price { get; set; }
        public DateTime receive_date { get; set; }
        //public int useful_life_year { get; set; }
        //public int asset_category_id { get; set; }
        //public string asset_category_name { get; set; } = string.Empty;
        //public int unit_id { get; set; }
        //public string unit_name { get; set; } = string.Empty;
        public int? fund_category_id { get; set; }
        public string? category_name { get; set; }
        public int? department_id { get; set; }
        public string? department_name { get; set; }
        public int? staff_id { get; set; }
        public string? staff_name { get; set; }
        public int? vendor_id { get; set; }
        public string? vendor_name { get; set; }
    }
    public class AssetItemCreateDto
    {
        public int? asset_id { get; set; }
        public int? procurement_record_id { get; set; }
        public int? item_no { get; set; }
        public string asset_code_prefix { get; set; } = string.Empty;
        public string asset_name { get; set; } = string.Empty;
        //public decimal quantity { get; set; }
        //public decimal unit_price { get; set; }
        //public decimal total_price { get; set; }
        public DateTime receive_date { get; set; }
        //public int useful_life_year { get; set; }
        //public int asset_category_id { get; set; }
        //public int unit_id { get; set; }
        public int? fund_category_id { get; set; }
        public int? department_id { get; set; }
        public int? staff_id { get; set; }
        public int? vendor_id { get; set; }
    }
}
