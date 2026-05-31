using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Expense_types : BaseEntity
    {
        [Key]
        public int expense_type_id { get; set; }
        public string expense_type_name { get; set; } = string.Empty;
    }
}
