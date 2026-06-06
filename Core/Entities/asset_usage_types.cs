using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class AssetUsageType : BaseEntity
    {
        [Key]
        public int usage_type_id { get; set; }
        public string usage_type_name { get; set; } = string.Empty;
    }
}
