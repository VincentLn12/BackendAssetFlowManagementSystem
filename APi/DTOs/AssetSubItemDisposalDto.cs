using System;

namespace APi.DTOs
{
    public class AssetSubItemDisposalDto
    {
        public int sub_item_disposal_id { get; set; }
        public int asset_sub_item_id { get; set; }
        public DateTime disposal_date { get; set; }
        public string disposal_method { get; set; } = string.Empty;
        public string? disposal_reason { get; set; }
        public string? document_no { get; set; }
        public string? approved_by { get; set; }
        public decimal quantity_disposed { get; set; }
        public string? notes { get; set; }
    }
}
