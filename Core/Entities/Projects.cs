using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    public class Projects : BaseEntity
    {
        [Key]
        public int project_id { get; set; }

        [MaxLength(100)]
        public string project_code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string project_name { get; set; } = string.Empty;


        [Column(TypeName = "decimal(18,2)")]
        public decimal project_budget_amount { get; set; }


        public bool is_active { get; set; } = true;

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public DateTime? updated_at { get; set; }

        public int fiscal_year_id { get; set; }
        [ForeignKey ("fiscal_year_id")]
        public Fiscal_years? fiscal_year { get; set; }
        public int? staff_id { get; set; }
        [ForeignKey("staff_id")]
        public Staffs? staff { get; set; }

    }
}
