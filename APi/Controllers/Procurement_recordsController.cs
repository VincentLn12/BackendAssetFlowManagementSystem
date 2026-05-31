
using Core.Interfaces.Specifications.Procurement_records;

namespace API.Controllers;

public class Procurement_recordsController(IUnitOfWork unit, IMapper mapper) : BaseApiController
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

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("File type not allowed");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/{fileName}";

        return Ok(new
        {
            fileName,
            filePath = relativePath
        });
    }


}
