using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class Procurement_records : BaseEntity
    {
        [Key]
        public int procurement_record_id { get; set; }
        public string document_no { get; set; } = string.Empty;
        public DateTime? document_date { get; set; } = DateTime.Now; //วันที่เอกสาร
        public DateTime? inspection_date { get; set; } 
        [Column(TypeName = "decimal(18,2)")]
        public decimal total_amount { get; set; }
        public string amount_text { get; set; } = string.Empty; //จำนวนเงินรวม(ตัวอักษร)
        public DateTime? approval_date { get; set; }  = DateTime.Now;
        public string? reference_no { get; set; } = string.Empty; //เลขที่อ้างอิง
        public string status { get; set; } = string.Empty; //สถานะ
        public string? remark { get; set; } = string.Empty; //หมายเหตุ
        public string? attachment_file_path { get; set; } //แนบไฟล์
        //ปีงบประมาณ
        public int fiscal_year_id { get; set; } 
        [ForeignKey("fiscal_year_id")]
        [JsonIgnore]
        public Fiscal_years fiscal_Years { get; set; } = null!;

        //ประเภทดำเนินการ
        public int operation_type_id { get; set; }
        [ForeignKey("operation_type_id")]
        [JsonIgnore]
        public Operation_types operation_Types { get; set; } = null!;

        //ประเภทค่าใช้จ่าย
        public int expense_type_id { get; set; }
        [ForeignKey("expense_type_id")]
        [JsonIgnore]
        public Expense_types expense_Types { get; set; } = null!;

        //คณะ
        public int department_id { get; set; }
        [ForeignKey("department_id")]
        [JsonIgnore]
        public Departments departments { get; set; } = null!;

        //บริษัท
        public int vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        [JsonIgnore]
        public Vendors vendors { get; set; } = null!;

        //หมวดเงิน
        public int fund_category_id { get; set; }
        [ForeignKey("fund_category_id")]
        [JsonIgnore]
        public Fund_categories fund_Categories { get; set; } = null!;

        //เเหล่งงบประมาณ
        public int budget_source_id { get; set; }
        [ForeignKey("budget_source_id")]
        [JsonIgnore]
        public Budget_sources budget_Sources { get; set; } = null!;

        //ผู้เบิกจ่าย
        public int staff_id { get; set; }
        [ForeignKey("staff_id")]
        [JsonIgnore]
        public Staffs staffs { get; set; } = null!;

        public int project_id { get; set; }
        [ForeignKey("project_id")]
        [JsonIgnore]
        public Projects projects { get; set; } = null!;

    }
}
