using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Prefixes : BaseEntity
    {
        [Key]
        public int prefix_id { get; set; }
        [Required]
        public string prefix_name { get; set; } = ""; //คำน้ำหน้าชื่อเต็ม
        public string prefix_short_name { get; set; } = ""; //คำน้ำหน้าชื่อย่อ
    }
}
