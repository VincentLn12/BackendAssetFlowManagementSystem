using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ProcurementRecordStatusHistory : BaseEntity
    {
        [Key]
        public int status_history_id { get; set; }
        public int procurement_record_id { get; set; }
        public string from_status { get; set; } = string.Empty;
        public string to_status { get; set; } = string.Empty;
        public DateTime changed_at { get; set; }
        public int? changed_by_staff_id { get; set; }
        public string? remark { get; set; }

        [ForeignKey("procurement_record_id")]
        public Procurement_records? ProcurementRecord { get; set; }

        [ForeignKey("changed_by_staff_id")]
        public Staffs? ChangedByStaff { get; set; }
    }
}
