using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class HireDetail : BaseEntity
    {
        [Key]
        public int hire_detail_id { get; set; }
        public int item_no { get; set; }

        public string hire_name { get; set; } = string.Empty;

        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }

        public decimal total_amount { get; set; }
        public string total_text { get; set; } = string.Empty;

        public string? operation_reason { get; set; }

        public string? remark { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public int procurement_record_id { get; set; }
        [ForeignKey("procurement_record_id")]
        public Procurement_records? procurement_record { get; set; }
        public int? unit_id { get; set; }
        [ForeignKey("unit_id")]
        public MaterialUnit? unit { get; set; }

    }
}
