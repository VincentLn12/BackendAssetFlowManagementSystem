using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MaterialWithdrawal : BaseEntity
    {
        [Key]
        public int material_withdrawal_id { get; set; }

        //เลขที่จัดซื้อ
        public string? material_receive_id { get; set; }  
        // เลขที่อ้างอิง
        public string? receive_document_no { get; set; }
        //เลขที่ใบเบิก
        public string? withdrawal_document_no { get; set; } = null;

        // ผู้เบิก
        public int staff_id { get; set; }
        [ForeignKey("staff_id")]
        public Staffs? staffs { get; set; }
        public int procurement_record_id { get; set; }
        [ForeignKey("procurement_record_id")]
        public Procurement_records? ProcurementRecord { get; set; }

        public string? remark { get; set; }
    }
}
