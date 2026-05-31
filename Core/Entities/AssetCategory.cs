using System.ComponentModel.DataAnnotations;
using Core.Entities;

public class AssetCategory : BaseEntity
{
    [Key]
    public int asset_category_id { get; set; }

    [Required]
    [MaxLength(255)]
    public string category_name { get; set; } = string.Empty;
}