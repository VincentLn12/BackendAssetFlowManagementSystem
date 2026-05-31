using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Fund_categories : BaseEntity
    {
        [Key]
        public int fund_category_id { get; set; }
        [MaxLength(50)]
        public string fund_code { get; set; } = string.Empty; //รหัสหมวดหมู่เงิน
        [MaxLength(255)]
        public string fund_name { get; set; } = string.Empty; //ชื่อหมวดหมู่เงิน
    }
}
