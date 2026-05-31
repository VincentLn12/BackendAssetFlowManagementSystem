

using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Vendors : BaseEntity
    {
        [Key]
        public int vendor_id { get; set; }
        [Required]
        [StringLength(100)]
        public string vendor_name { get; set; } = string.Empty;
        public string tax_no { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string contact_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
