namespace APi.DTOs
{
    public class MaterialWithdrawalDto
    {
        public int material_withdrawal_id { get; set; }
        //เลขที่จัดซื้อ
        public string? material_receive_id { get; set; }
        // เลขที่อ้างอิง
        public string? receive_document_no { get; set; }
        //เลขที่ใบเบิก
        public string? withdrawal_document_no { get; set; } = null;
        public int staff_id { get; set; }
        public string? staff_name { get; set; }
        public int procurement_record_id { get; set; }
        public string? remark { get; set; }

    }
    public class MaterialWithdrawalCreateDto
    {
        public int material_withdrawal_id { get; set; }
        //เลขที่จัดซื้อ
        public string? material_receive_id { get; set; }
        // เลขที่อ้างอิง
        public string? receive_document_no { get; set; }
        //เลขที่ใบเบิก
        public string? withdrawal_document_no { get; set; } = null;
        public int staff_id { get; set; }
        public int procurement_record_id { get; set; }
        public string? remark { get; set; }

    }
}
