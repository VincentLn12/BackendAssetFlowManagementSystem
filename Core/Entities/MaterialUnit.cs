using System.ComponentModel.DataAnnotations;
using Core.Entities;

public class MaterialUnit : BaseEntity
{
    [Key]
    public int unit_id { get; set; }
    [Required]
    [MaxLength(100)]
    public string unit_name { get; set; } = string.Empty;
   
}