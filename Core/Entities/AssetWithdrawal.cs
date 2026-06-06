using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class AssetWithdrawal : BaseEntity
    {
        [Key]
        public int procurement_withdrawal_id { get; set; }
        public int procurement_record_id { get; set; }

        [MaxLength(100)]
        public string withdrawal_document_no { get; set; } = string.Empty;
        [Column(TypeName = "date")]
        public DateTime withdrawal_date { get; set; }
        public int staff_id { get; set; }

        [MaxLength(255)]
        public string storage_location { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? purpose { get; set; }

        public string? remark { get; set; }

        [ForeignKey(nameof(procurement_record_id))]
        public Procurement_records? ProcurementRecord { get; set; }

        [ForeignKey(nameof(staff_id))]
        public Staffs? Staff { get; set; }
     
    }

}
