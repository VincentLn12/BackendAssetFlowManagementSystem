namespace APi.DTOs
{
    public class ProcurementMaterialFullCreateDto
    {
        public ProcurementRecordCreateDto procurement_record { get; set; } = new();

        public List<MaterialReceiveDetailDto> material_receive_details { get; set; } = new();
    }
}
