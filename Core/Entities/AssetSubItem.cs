using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;


    public class AssetSubItem : BaseEntity
    {
        [Key]
        public int asset_sub_item_id { get; set; }

        public int asset_id { get; set; }
        [ForeignKey("asset_id")]
        public AssetItem? assetItem { get; set; }

        public int? item_no { get; set; }

        [Required]
        [StringLength(500)]
        public string sub_item_name { get; set; } = string.Empty;

        public int asset_category_id { get; set; }
        [ForeignKey("asset_category_id")]
        public AssetCategory? asset_category { get; set; }

        public int running_start_no { get; set; }

        public int running_end_no { get; set; }

        public int fiscal_asset_year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal quantity { get; set; }
        public int unit_id { get; set; }
        [ForeignKey("unit_id")]
        public MaterialUnit? materialUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? unit_price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? total_price { get; set; }

        public int useful_life_year { get; set; }
    }
