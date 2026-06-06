
namespace APi.DTOs
{
    public class AssetItemDto
    {
        public int asset_id { get; set; }
        public int? procurement_record_id { get; set; }
        public int? item_no { get; set; }
        public string asset_code_prefix { get; set; } = string.Empty;
        public string asset_name { get; set; } = string.Empty;
        public DateTime receive_date { get; set; }
        public int? fund_category_id { get; set; }
        public string? category_name { get; set; }
        public int? department_id { get; set; }
        public string? department_name { get; set; }
        ////public int? staff_id { get; set; }
        ////public string? staff_name { get; set; }
        //public int? vendor_id { get; set; }
        //public string? vendor_name { get; set; }
        public int? acquisition_method_id { get; set; }
        public string acquisition_method_name { get;set; } = string.Empty;
    }

    public class AssetItemDetailsDto
    {
        public int? asset_id { get; set; }
        public string project_code { get; set; } = string.Empty;
        public string staff_name { get; set; } = string.Empty;
        public string department_name { get; set; } = string.Empty;
        public string vendor_name { get; set; } = string.Empty;
        public string vendor_address { get; set; } = string.Empty;
        public string vendor_tel { get; set; } = string.Empty;
        public string fund_name { get; set; } = string.Empty;
        public string acquisition_method_name { get; set; } = string.Empty;
        public DateTime receive_date { get; set; }

        public List<AssetSubItemDto> asset_sub_items { get; set; } = new();
    }
    public class AssetItemCreateDto
    {
        public int? asset_id { get; set; }
        public int? procurement_record_id { get; set; }
        public int? item_no { get; set; }
        public string? asset_code_prefix { get; set; } = string.Empty;
        public string asset_name { get; set; } = string.Empty;      
        public DateTime receive_date { get; set; }
        public int? fund_category_id { get; set; }
        public int? department_id { get; set; }       
        public int? acquisition_method_id { get; set; }

    }
}
