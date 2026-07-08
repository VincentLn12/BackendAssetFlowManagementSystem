namespace APi.DTOs
{
    public class HireDetailDto
    {
        public int hire_detail_id { get; set; }
        public int procurement_record_id { get; set; }
        public string document_no { get; set; } = string.Empty;
        public int item_no { get; set; }
        public string hire_name { get; set; } = string.Empty;
        public int unit_id { get; set; }
        public string unit_name { get; set; } = string.Empty;
        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_amount { get; set; }
        public string total_text { get; set; } = string.Empty;
        public string? operation_reason { get; set; }
        public string? remark { get; set; }
    }
    public class HireDetailCreateDto
    {
        public int? hire_detail_id { get; set; }
        public int procurement_record_id { get; set; }
        public int item_no { get; set; }
        public string hire_name { get; set; } = string.Empty;
        public decimal quantity { get; set; }
        public int? unit_id { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_amount { get; set; }
        public string total_text { get; set; } = string.Empty;
        public string? operation_reason { get; set; }
        public string? remark { get; set; }
    }
}
