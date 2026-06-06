
namespace APi.DTOs
{
    public class MaterialItemDto
    {  
        public int material_item_id { get; set; }
        public string? material_code { get; set; } = string.Empty;
        public string material_name { get; set; } = string.Empty;
        public string? specification { get; set; }
        public int unit_id { get; set; }
        public string? unit_name { get; set; } = string.Empty;
        public decimal? opening_balance { get; set; }
        public decimal? quantity_in { get; set; }
        public decimal? quantity_out { get; set; }
        public decimal? current_balance { get; set; }
        public decimal? unit_price { get; set; }
        public decimal? total_amount { get; set; }
        public string? remark { get; set; }
        public decimal? min_stock { get; set; }     
    }
}
