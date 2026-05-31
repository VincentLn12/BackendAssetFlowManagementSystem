
namespace APi.DTOs
{
    public class ProcurementRecordDto
    {
        public int procurement_record_id { get; set; }
        public string document_no { get; set; } = string.Empty;
        public DateTime document_date { get; set; } 
        public DateTime inspection_date { get; set; } 
        public decimal total_amount { get; set; }
        public string amount_text { get; set; } = string.Empty; 
        public DateTime approval_date { get; set; } 
        public string? reference_no { get; set; } = string.Empty; 
        public string status { get; set; } = string.Empty; 
        public string? remark { get; set; } = string.Empty; 
        public int project_id { get; set; }
        public string project_code { get; set; } = string.Empty;
        public string? attachment_file_path { get; set; }
        public int fiscal_year_id { get; set; }
        public string fiscal_year_name { get; set; } = string.Empty;
        public int operation_type_id { get; set; }
        public string operation_type_name { get; set; } = string.Empty;
        public int expense_type_id { get; set; }
        public string expense_type_name { get; set; } = string.Empty;
        public int department_id { get; set; }
        public string department_name { get; set; } = string.Empty;
        public int vendor_id { get; set; }
        public string vendor_name { get; set; } = string.Empty;
        public int fund_category_id { get; set; }
        public string fund_category_name { get; set; } = string.Empty;
        public int budget_source_id { get; set; }
        public string budget_source_name { get; set; } = string.Empty;
        public int staff_id { get; set; }
        public string staff_fullname { get; set; } = string.Empty;
    }
    public class ProcurementRecordCreateDto
    {
        public int? procurement_record_id { get; set; }
        public string document_no { get; set; } = string.Empty;
        public DateTime document_date { get; set; }
        public DateTime inspection_date { get; set; }
        public decimal total_amount { get; set; }
        public string amount_text { get; set; } = string.Empty;
        public DateTime? approval_date { get; set; }
        public string? reference_no { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string? remark { get; set; } = string.Empty;
        public int project_id { get; set; } 
        public string? attachment_file_path { get; set; }
        public int fiscal_year_id { get; set; }
        public int operation_type_id { get; set; }
        public int expense_type_id { get; set; }
        public int department_id { get; set; }
        public int vendor_id { get; set; }
        public int fund_category_id { get; set; }
        public int budget_source_id { get; set; }
        public int staff_id { get; set; }
    }
}
