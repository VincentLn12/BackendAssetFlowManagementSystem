using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class AcquisitionMethod : BaseEntity
    {
        [Key]
        public int acquisition_method_id { get; set; }

        public string acquisition_method_name { get; set; } = string.Empty;

    }
}
