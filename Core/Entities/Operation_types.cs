using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Operation_types : BaseEntity
    {
        [Key]
        public int operation_type_id { get; set; }
        public string operation_type_name { get; set; } = string.Empty;
    } 
}
