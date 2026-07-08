
namespace APi.DTOs
{
    public class MaterialIssueDetailDto
    {
        public int issue_detail_id { get; set; }
        public int? procurement_record_id { get; set; }
        public int? department_id { get; set; }
        public int material_item_id { get; set; }
        public int? staff_id { get; set; }
        public string? staff_fullname { get; set; } = null;
        public DateTime? issue_date { get; set; }
        public decimal quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal? total_amount { get; set; }
        public string? remark { get; set; }

    }
    public class MaterialIssueDetailManyCreateDto
    {
        public List<MaterialIssueDetailDto> items { get; set; } = [];
    }
}
