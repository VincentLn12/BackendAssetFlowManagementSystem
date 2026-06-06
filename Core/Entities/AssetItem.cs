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
    public string? asset_code_prefix { get; set; } = string.Empty;

    [MaxLength(500)]
    public string asset_name { get; set; } = string.Empty;

    [Column(TypeName = "date")]
    public DateTime receive_date { get; set; }
    public int? fund_category_id { get; set; }
    [ForeignKey("fund_category_id")]
    public Fund_categories? FundCategory { get; set; }
    public int? department_id { get; set; }
    [ForeignKey("department_id")]
    public Departments? Department { get; set; }
    public int? acquisition_method_id { get; set; }
    [ForeignKey("acquisition_method_id")]
    public AcquisitionMethod? AcquisitionMethod { get; set; }

    [NotMapped]
    public AssetSubItem? AssetSubItem { get; set; }
    [NotMapped]
    public Procurement_records? Procurement_records { get; set; }
    [NotMapped]
    public AssetCategory? AssetCategory { get; set; }



}