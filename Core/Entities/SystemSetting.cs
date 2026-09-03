using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class SystemSetting : BaseEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string project_name { get; set; } = string.Empty;

        [Required]
        public string logo_path { get; set; } = string.Empty;
    }
}
