using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MaterialStockCard : BaseEntity
    {
        [Key]
        public int stock_card_id { get; set; }

        public int material_item_id { get; set; }
        public int? fiscal_year_id { get; set; }
        public int? department_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime transaction_date { get; set; }

        [StringLength(20)]
        public string transaction_type { get; set; } = string.Empty;

        [StringLength(100)]
        public string? reference_document_no { get; set; }
        public int? procurement_record_id { get; set; }

        public int? receive_detail_id { get; set; }
        public string? StaffName { get; set; } = string.Empty;

        public int? issue_detail_id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity_in { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity_out { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal balance_qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal unit_price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal total_amount { get; set; }

        [ForeignKey("material_item_id")]
        public MaterialItem? MaterialItem { get; set; }

        [ForeignKey("receive_detail_id")]
        public MaterialReceiveDetail? ReceiveDetail { get; set; }

        [ForeignKey("issue_detail_id")]
        public MaterialIssueDetail? IssueDetail { get; set; }

        [ForeignKey("fiscal_year_id")]
        public Fiscal_years? fiscal_Years { get; set; }
        [ForeignKey("department_id")]
        public Departments? Department { get; set; }

    }
}
