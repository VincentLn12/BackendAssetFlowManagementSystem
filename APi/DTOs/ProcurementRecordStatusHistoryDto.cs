namespace APi.DTOs
{
    public class UpdateProcurementRecordStatusDto
    {
        public string to_status { get; set; } = string.Empty;
        public int? changed_by_staff_id { get; set; }
        public string? remark { get; set; }
    }

    public class ProcurementRecordStatusHistoryDto
    {
        public int status_history_id { get; set; }
        public int procurement_record_id { get; set; }
        public string from_status { get; set; } = string.Empty;
        public string to_status { get; set; } = string.Empty;
        public DateTime changed_at { get; set; }
        public int? changed_by_staff_id { get; set; }
        public string? changed_by_staff_name { get; set; }
        public string? remark { get; set; }
    }
}
