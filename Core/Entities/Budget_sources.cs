using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Budget_sources : BaseEntity
    {
        [Key]
        public int budget_source_id { get; set; }
        [Required]
        public string budget_source_name { get; set; } = string.Empty;
    }
}
