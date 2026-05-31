using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Positions : BaseEntity
    {
        [Key]
        public int position_id { get; set; }
        [Required]
        [StringLength(255)]
        public string position_name { get; set; } = string.Empty;

    }
}
