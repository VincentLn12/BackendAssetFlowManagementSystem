using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    public class MaterialItem : BaseEntity
    {
        [Key]
        public int material_item_id { get; set; }

        [StringLength(100)]
        public string? material_code { get; set; } = string.Empty;

        [StringLength(500)]
        public string material_name { get; set; } = string.Empty;
        public string? specification { get; set; }

      
        [Column(TypeName = "decimal(18,2)")]
        public decimal? opening_balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? quantity_in { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? quantity_out { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? current_balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? unit_price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? total_amount { get; set; }
        public string? remark { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? min_stock { get; set; }

        // Navigation Properties
        public int unit_id { get; set; }
        [ForeignKey("unit_id")]
        public MaterialUnit? Unit { get; set; }
    }
}
