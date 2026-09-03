
namespace APi.DTOs
{
    public class AssetWithdrawalDto
    {
        public int procurement_withdrawal_id { get; set; }
        public int procurement_record_id { get; set; }
        public string withdrawal_document_no { get; set; } = string.Empty;
        public int staff_id { get; set; }
        public string staff_name { get; set; } = string.Empty;
        public string storage_location { get; set; } = string.Empty;
        public string? purpose { get; set; }
        public string? remark { get; set; }
        public DateTime withdrawal_date { get; set; }
        public DateTime? end_date { get; set; }
        public string? end_reason { get; set; }

    }
    public class AssetWithdrawalCreateDto
    {
        public int procurement_withdrawal_id { get; set; }
        public int procurement_record_id { get; set; }
        public int staff_id { get; set; }
        public string storage_location { get; set; } = string.Empty;
        public string? purpose { get; set; }
        public string? remark { get; set; }
        public DateTime withdrawal_date { get; set; }
        public DateTime? end_date { get; set; }
        public string? end_reason { get; set; }

    }
}
