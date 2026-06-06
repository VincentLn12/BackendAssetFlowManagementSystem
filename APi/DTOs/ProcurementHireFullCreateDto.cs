namespace APi.DTOs
{
    public class ProcurementHireFullCreateDto
    {
        public ProcurementRecordCreateDto procurement_record { get; set; } = new();
        public List<HireDetailCreateDto> hire_details { get; set; } = new();
    }
}
