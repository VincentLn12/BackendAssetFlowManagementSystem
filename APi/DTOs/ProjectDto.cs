namespace APi.DTOs
{
    public class ProjectDto
    {  
        public int project_id { get; set; }

        public string project_code { get; set; } = string.Empty;

        public string project_name { get; set; } = string.Empty;

        public int fiscal_year_id { get; set; }
        public string fiscal_year_name { get; set; } = string.Empty;

        public decimal project_budget_amount { get; set; }

        public int? staff_id { get; set; }
        public string staff_name { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;

        public bool is_active { get; set; } = true;

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public DateTime? updated_at { get; set; }
    }
  
     public class ProjectAddUpdateDto
    {
        public int? project_id { get; set; }
        public string project_code { get; set; } = string.Empty;
        public string project_name { get; set; } = string.Empty;
        public int fiscal_year_id { get; set; }
        public decimal project_budget_amount { get; set; }
        public int? staff_id { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime? updated_at { get; set; }
        public string filePath { get; set; } = string.Empty;

    }
}
