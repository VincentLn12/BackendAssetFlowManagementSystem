namespace APi.DTOs
{
    public class MaterialStockCardDto
    {
        public int stock_card_id { get; set; }
        public int material_item_id { get; set; }
        public DateTime transaction_date { get; set; }
        public string transaction_type { get; set; } = string.Empty;
        public string? reference_document_no { get; set; }
        public int? procurement_record_id { get; set; }
        public int? fiscal_year_id { get; set; }
        public int? department_id { get; set; }

        public decimal quantity_in { get; set; }
        public decimal quantity_out { get; set; }
        public decimal balance_qty { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_amount { get; set; }
        public string? staff_name { get; set; } = string.Empty;
    }
}
