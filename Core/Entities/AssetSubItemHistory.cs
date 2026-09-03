
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;

namespace Core.Entities
{
    public class AssetSubItemHistory : BaseEntity
    {
        [Key]
        public int sub_item_history_id { get; set; }

        public int procurement_withdrawal_id { get; set; }

        public DateTime history_date { get; set; }
        public string history_type { get; set; } = string.Empty;

        public int usage_type_id { get; set; }

        public string? detail { get; set; }

        // Navigation
        [ForeignKey(nameof(procurement_withdrawal_id))]
        public AssetWithdrawal? AssetWithdrawal { get; set; }
        [ForeignKey(nameof(usage_type_id))]
        public AssetUsageType? AssetUsageType { get; set; }
        public int? staff_id { get; set; }
        [ForeignKey(nameof(staff_id))]
        public Staffs? Staff { get; set; }

    }
}
