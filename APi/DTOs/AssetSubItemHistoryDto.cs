namespace APi.DTOs
{
    public class AssetSubItemHistoryDto
    {
        public int sub_item_history_id { get; set; }
        public int procurement_withdrawal_id { get; set; }
        public int? staff_id { get; set; }
        public string  history_date { get; set; } = string.Empty;
        public string history_type { get; set; } = string.Empty;
        public int usage_type_id { get; set; }
        public string? usage_type_name { get; set; } 
        public string? detail { get; set; }
        public string? FullName { get; set; } = string.Empty;

    }
}
