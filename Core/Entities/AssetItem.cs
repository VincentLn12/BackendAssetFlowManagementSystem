using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;

public class AssetItem : BaseEntity
{
    [Key]
    public int asset_id { get; set; }

    public int? procurement_record_id { get; set; }

    public int? item_no { get; set; }

    [MaxLength(100)]
    public string asset_code_prefix { get; set; } = string.Empty;

    [MaxLength(500)]
    public string asset_name { get; set; } = string.Empty;

    //[Column(TypeName = "decimal(18,2)")]
    //public decimal quantity { get; set; }

    //[Column(TypeName = "decimal(18,2)")]
    //public decimal unit_price { get; set; }

    //[Column(TypeName = "decimal(18,2)")]
    //public decimal total_price { get; set; }

    [Column(TypeName = "date")]
    public DateTime receive_date { get; set; }

    //public int useful_life_year { get; set; }

    //relationships
    //public int asset_category_id { get; set; }
    //[ForeignKey("asset_category_id")]
    //public AssetCategory? AssetCategory { get; set; }
    //public int unit_id { get; set; }
    //[ForeignKey("unit_id")]
    //public MaterialUnit? Unit { get; set; }
    public int? fund_category_id { get; set; }
    [ForeignKey("fund_category_id")]
    public Fund_categories? FundCategory { get; set; }
    public int? department_id { get; set; }
    [ForeignKey("department_id")]
    public Departments? Department { get; set; }
    public int? staff_id { get; set; }
    [ForeignKey("staff_id")]
    public Staffs? Staff { get; set; }
    public int? vendor_id { get; set; }
    [ForeignKey("vendor_id")]
    public Vendors? Vendor { get; set; }


}