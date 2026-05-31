using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Fiscal_years : BaseEntity
    {
        [Key]
        public int fiscal_year_id { get; set; }
        [Required]
        public int fiscal_year { get; set; }
        public string year_name { get; set; } = string.Empty;
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public bool is_closed { get; set; }
        public DateTime? closed_at { get; set; }
    }
}
