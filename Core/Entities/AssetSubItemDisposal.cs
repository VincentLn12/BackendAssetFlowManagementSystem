using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class AssetSubItemDisposal : BaseEntity
    {
        [Key]
        public int sub_item_disposal_id { get; set; }

        public int asset_sub_item_id { get; set; }

        [ForeignKey("asset_sub_item_id")]
        public AssetSubItem? asset_sub_item { get; set; }

        [Required]
        public DateTime disposal_date { get; set; }

        [Required]
        [MaxLength(200)]
        public string disposal_method { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? disposal_reason { get; set; }

        [MaxLength(200)]
        public string? document_no { get; set; }

        [MaxLength(200)]
        public string? approved_by { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity_disposed { get; set; }

        [MaxLength(1000)]
        public string? notes { get; set; }
    }
}
