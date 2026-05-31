using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class Staffs : BaseEntity
    {
        [Key]
        public int staff_id { get; set; }
        [Required]
        public string first_name { get; set; } = string.Empty;
        [Required]
        public string last_name { get; set; } = string.Empty;
        [MaxLength(120)]
        public string? email { get; set; }
        [MaxLength(20)]
        public string? phone { get; set; }


        [ForeignKey("Departments")]
        public int department_id { get; set; }
        [JsonIgnore]
        public Departments Departments { get; set; } = null!;

        [ForeignKey("Positions")]
        public int position_id { get; set; }
        [JsonIgnore]
        public Positions Positions { get; set; } = null!;

        [ForeignKey("Prefixes")]
        public int prefix_id { get; set; }
        [JsonIgnore]
        public Prefixes Prefixes { get; set; } = null!;
    }
}
