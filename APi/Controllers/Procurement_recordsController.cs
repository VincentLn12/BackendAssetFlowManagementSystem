
using Core.Interfaces.Specifications.Procurement_records;

namespace API.Controllers;

public class Procurement_recordsController(IUnitOfWork unit, IMapper mapper, FileService fileService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Procurement_records>>> GetProcurementRecords([FromQuery] Procurement_recordsSpecParams procurementRecordsParams)
    {
        var spec = new Procurement_recordsSpecification(procurementRecordsParams);

        var procurementRecords = await unit.Repository<Procurement_records>().ListAsync(spec);

        var countSpec = new Procurement_recordForCountSpecification(procurementRecordsParams);

        var totalItems = await unit.Repository<Procurement_records>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<ProcurementRecordDto>>(procurementRecords);

        return Ok(new Pagination<ProcurementRecordDto>(
            procurementRecordsParams.PageIndex,
            procurementRecordsParams.PageSize,
            totalItems,
            data
        ));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Procurement_records>> GetProcurementRecord(int id)
    {
        var procurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (procurementRecord == null) return NotFound();
        return procurementRecord;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Procurement_records>> CreateProcurementRecord(ProcurementRecordCreateDto procurementRecord)
    {
        var procurement = mapper.Map<Procurement_records>(procurementRecord);

        procurement.is_active = true;
        procurement.amount_text =
                        ThaiBahtTextConverter.ToThaiBahtText(procurement.total_amount);

        procurement.approval_date = DateTime.UtcNow;
        procurement.created_at = DateTime.UtcNow;
       
        unit.Repository<Procurement_records>().Add(procurement);

        if (await unit.Complete())
        {
            return CreatedAtAction(
                nameof(GetProcurementRecord),
                new { id = procurement.procurement_record_id },
                procurement
            );
        }
        return BadRequest("Problem creating procurement record");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProcurementRecord(int id, ProcurementRecordCreateDto dto)
    {
        if (id != dto.procurement_record_id)
            return BadRequest("Cannot update this procurement record");

        var existingProcurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (existingProcurementRecord == null)
            return NotFound("ProcurementRecord not found");

        mapper.Map(dto, existingProcurementRecord);

        existingProcurementRecord.updated_at = DateTime.Now;

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the procurement record");
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProcurementRecord(int id)
    {
        var procurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (procurementRecord == null) return NotFound();

        procurementRecord.is_active = false;
        procurementRecord.updated_at = DateTime.UtcNow;

        unit.Repository<Procurement_records>().Update(procurementRecord);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting procurement record");
    }

    [HttpPost("create-with-assets")]
    public async Task<ActionResult> CreateProcurementWithAssets(ProcurementAssetFullCreateDto dto)
    {
        var procurement = mapper.Map<Procurement_records>(dto.procurement_record);

        procurement.is_active = true;
        procurement.amount_text = ThaiBahtTextConverter.ToThaiBahtText(procurement.total_amount);
        procurement.created_at = DateTime.UtcNow;
        procurement.approval_date = DateTime.UtcNow;

        unit.Repository<Procurement_records>().Add(procurement);

        if (!await unit.Complete())
            return BadRequest("Problem creating procurement record");

        var assetItem = mapper.Map<AssetItem>(dto.asset_item);

        assetItem.procurement_record_id = procurement.procurement_record_id;
        assetItem.is_active = true;
        assetItem.created_at = DateTime.UtcNow;

        unit.Repository<AssetItem>().Add(assetItem);

        if (!await unit.Complete())
            return BadRequest("Problem creating asset item");

        foreach (var subDto in dto.asset_sub_items)
        {
            var subItem = mapper.Map<AssetSubItem>(subDto);

            subItem.asset_id = assetItem.asset_id;
            subItem.is_active = true;
            subItem.created_at = DateTime.UtcNow;

            unit.Repository<AssetSubItem>().Add(subItem);
        }

        if (await unit.Complete())
        {
            return Ok(new
            {
                procurement_record_id = procurement.procurement_record_id,
                asset_id = assetItem.asset_id,
                message = "Create procurement with asset successfully"
            });
        }

        return BadRequest("Problem creating asset sub items");
    }
    [HttpPost("create-with-hire")]
    public async Task<ActionResult> CreateProcurementWithHire(
    [FromBody] ProcurementHireFullCreateDto dto
)
    {
        if (dto.hire_details == null || dto.hire_details.Count == 0)
            return BadRequest("กรุณาเพิ่มรายการจัดจ้างอย่างน้อย 1 รายการ");

        var procurement = mapper.Map<Procurement_records>(dto.procurement_record);

        procurement.is_active = true;
        procurement.amount_text = ThaiBahtTextConverter.ToThaiBahtText(procurement.total_amount);
        procurement.created_at = DateTime.UtcNow;

        if (procurement.status == "ดำเนินการแล้ว")
            procurement.approval_date = DateTime.UtcNow;
        else
            procurement.approval_date = null;

        unit.Repository<Procurement_records>().Add(procurement);

        if (!await unit.Complete())
            return BadRequest("Problem creating procurement record");

        foreach (var hireDto in dto.hire_details)
        {
            var hireDetail = mapper.Map<HireDetail>(hireDto);

            hireDetail.procurement_record_id = procurement.procurement_record_id;
            hireDetail.total_amount = hireDetail.quantity * hireDetail.unit_price;
            hireDetail.total_text = ThaiBahtTextConverter.ToThaiBahtText(hireDetail.total_amount);
            hireDetail.is_active = true;

            unit.Repository<HireDetail>().Add(hireDetail);
        }

        if (await unit.Complete())
        {
            return Ok(new
            {
                procurement_record_id = procurement.procurement_record_id,
                message = "Create procurement with hire details successfully"
            });
        }

        return BadRequest("Problem creating hire details");
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            var filePath = await fileService.UploadFileAsync(file, "uploads");
            return Ok(new
            {
                filePath
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
