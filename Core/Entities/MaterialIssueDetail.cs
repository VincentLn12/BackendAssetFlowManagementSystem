using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    public class MaterialIssueDetail : BaseEntity
    {
        [Key]
        public int issue_detail_id { get; set; }

        public int?     procurement_record_id { get; set; }
        public int material_item_id { get; set; }

        public int? staff_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? issue_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal unit_price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? total_amount { get; set; }

        public string? remark { get; set; }

        [ForeignKey("procurement_record_id")]
        public Procurement_records? ProcurementRecord { get; set; }

        [ForeignKey("material_item_id")]
        public MaterialItem? MaterialItem { get; set; }

        [ForeignKey("staff_id")]
        public Staffs? Requester { get; set; }
      
    }
}
