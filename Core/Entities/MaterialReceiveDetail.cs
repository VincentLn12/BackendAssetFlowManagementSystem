using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    public class MaterialReceiveDetail : BaseEntity
    {
        [Key]
        public int receive_detail_id { get; set; }

        public int procurement_record_id { get; set; }

        public int item_no { get; set; }

        public int material_item_id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal unit_price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? total_amount { get; set; }

        public string? operation_reason { get; set; }

        [ForeignKey("procurement_record_id")]
        public Procurement_records? ProcurementRecord { get; set; }

        [ForeignKey("material_item_id")]
        public MaterialItem? MaterialItem { get; set; }
    }
}
