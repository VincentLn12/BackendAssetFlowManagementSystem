
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Departments :BaseEntity
    {
        [Key]
        public int department_id { get; set; }
        [Required]
        public string department_name { get; set; } = "";
    }
}
