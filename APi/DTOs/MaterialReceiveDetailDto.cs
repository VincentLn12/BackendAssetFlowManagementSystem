namespace APi.DTOs
{
    public class MaterialReceiveDetailDto
    {  
        public int receive_detail_id { get; set; }
        public int procurement_record_id { get; set; }
        public int item_no { get; set; }
        public int material_item_id { get; set; }
        public string? material_name { get; set; }
        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal? total_amount { get; set; }
        public string? operation_reason { get; set; }
    }
    public class MaterialReceiveDetailCreateDto
    {
        public int? receive_detail_id { get; set; }
        public int procurement_record_id { get; set; }
        public int item_no { get; set; }
        public int material_item_id { get; set; }
        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal? total_amount { get; set; }
        public string? operation_reason { get; set; }
    }
}
