namespace APi.DTOs
{
    public class StaffDto
    {
        public int staff_id { get; set; }

        public string first_name { get; set; } = string.Empty;

        public string last_name { get; set; } = string.Empty;

        public string full_name { get; set; } = string.Empty;

        public string? email { get; set; }

        public string? phone { get; set; }

        public int department_id { get; set; }

        public string? department_name { get; set; }

        public int position_id { get; set; }

        public string? position_name { get; set; }

        public int prefix_id { get; set; }

        public string? prefix_name { get; set; }
    }
    public class StaffCreateDto
    {
        public int staff_id { get; set; }
        public string first_name { get; set; } = string.Empty;

        public string last_name { get; set; } = string.Empty;   
        public string? email { get; set; }

        public string? phone { get; set; }

        public int department_id { get; set; }    

        public int position_id { get; set; }    

        public int prefix_id { get; set; }

        
    }
}
