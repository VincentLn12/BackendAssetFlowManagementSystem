namespace APi.DTOs
{
    public class AssetRepairDto
    {
        public int asset_repair_id { get; set; }
        public string repair_document_no { get; set; } = string.Empty;
        public DateTime repair_date { get; set; }
        public string? problem_description { get; set; } = string.Empty;
        public string? repair_description { get; set; }
        public string? repair_shop_name { get; set; }
        public decimal? repair_cost { get; set; }
        public string? decree_document_no { get; set; }
        public string status { get; set; } = string.Empty;
        public int procurement_withdrawal_id { get; set; }
        public int? staff_id { get; set; }
        public string? FullName { get; set; } = string.Empty;

    }
}
