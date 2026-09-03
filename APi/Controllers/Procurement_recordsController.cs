
using Core.Interfaces.Specifications.Procurement_records;

namespace API.Controllers;

public class Procurement_recordsController(
    IUnitOfWork unit,
    IMapper mapper,
    FileService fileService,
    StoreContext context
) : BaseApiController
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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateProcurementRecordStatus(int id, UpdateProcurementRecordStatusDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.to_status))
            return BadRequest("กรุณาระบุสถานะใหม่");

        var procurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (procurementRecord == null)
            return NotFound("Procurement record not found");

        if (!procurementRecord.is_active)
            return BadRequest("ไม่สามารถเปลี่ยนสถานะเอกสารที่ถูกลบแล้ว");

        var currentStatus = procurementRecord.status?.Trim() ?? string.Empty;
        var nextStatus = dto.to_status.Trim();

        if (string.Equals(currentStatus, nextStatus, StringComparison.Ordinal))
            return BadRequest("สถานะใหม่ต้องไม่ซ้ำกับสถานะปัจจุบัน");

        if (dto.changed_by_staff_id.HasValue)
        {
            var staff = await unit.Repository<Staffs>().GetByIdAsync(dto.changed_by_staff_id.Value);
            if (staff == null || !staff.is_active)
                return BadRequest("ไม่พบข้อมูลเจ้าหน้าที่ผู้เปลี่ยนสถานะ");
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            procurementRecord.status = nextStatus;
            procurementRecord.updated_at = DateTime.UtcNow;
            procurementRecord.approval_date = nextStatus == "ดำเนินการแล้ว"
                ? procurementRecord.approval_date ?? DateTime.UtcNow
                : procurementRecord.approval_date;

            unit.Repository<Procurement_records>().Update(procurementRecord);

            var history = new ProcurementRecordStatusHistory
            {
                procurement_record_id = procurementRecord.procurement_record_id,
                from_status = currentStatus,
                to_status = nextStatus,
                changed_at = DateTime.UtcNow,
                changed_by_staff_id = dto.changed_by_staff_id,
                remark = dto.remark,
                is_active = true,
                created_at = DateTime.UtcNow
            };

            unit.Repository<ProcurementRecordStatusHistory>().Add(history);

            if (!await unit.Complete())
            {
                await transaction.RollbackAsync();
                return BadRequest("Problem updating procurement record status");
            }

            await transaction.CommitAsync();
            return NoContent();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    [HttpGet("{id}/status-history")]
    public async Task<ActionResult<IReadOnlyList<ProcurementRecordStatusHistoryDto>>> GetProcurementRecordStatusHistory(int id)
    {
        var procurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (procurementRecord == null)
            return NotFound("Procurement record not found");

        var histories = await context.procurementRecordStatusHistories
            .AsNoTracking()
            .Where(x => x.is_active && x.procurement_record_id == id)
            .Include(x => x.ChangedByStaff!)
                .ThenInclude(x => x.Prefixes)
            .OrderByDescending(x => x.changed_at)
            .ThenByDescending(x => x.status_history_id)
            .Select(x => new ProcurementRecordStatusHistoryDto
            {
                status_history_id = x.status_history_id,
                procurement_record_id = x.procurement_record_id,
                from_status = x.from_status,
                to_status = x.to_status,
                changed_at = x.changed_at,
                changed_by_staff_id = x.changed_by_staff_id,
                changed_by_staff_name = x.ChangedByStaff != null
                    ? $"{x.ChangedByStaff.Prefixes!.prefix_name}{x.ChangedByStaff.first_name} {x.ChangedByStaff.last_name}"
                    : null,
                remark = x.remark
            })
            .ToListAsync();

        return Ok(histories);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProcurementRecord(int id)
    {
        var procurementRecord = await unit.Repository<Procurement_records>().GetByIdAsync(id);

        if (procurementRecord == null)
            return NotFound();

        if (!procurementRecord.is_active)
            return BadRequest("เอกสารนี้ถูกลบไปแล้ว");

        // หา MaterialReceiveDetail ของเอกสารนี้
        var receiveDetails = await unit.Repository<MaterialReceiveDetail>()
                 .ListAsync(new MaterialReceiveDetailByProcurementForDeleteSpec(id));

        foreach (var detail in receiveDetails)
        {
            var materialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(detail.material_item_id);

            if (materialItem == null)
                return BadRequest($"Material item not found: {detail.material_item_id}");

            var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
            var oldBalance = stockCards
                .Where(x =>
                    x.is_active &&
                    x.material_item_id == detail.material_item_id &&
                    x.department_id == procurementRecord.department_id
                )
                .OrderByDescending(x => x.transaction_date)
                .ThenByDescending(x => x.stock_card_id)
                .Select(x => x.balance_qty)
                .FirstOrDefault();
            var newBalance = oldBalance - detail.quantity;

            if (newBalance < 0)
                return BadRequest($"ไม่สามารถลบได้ เนื่องจากวัสดุ {materialItem.material_name} มียอดคงเหลือไม่พอ");

            // หักยอดใน MaterialItem
            materialItem.quantity_in = (materialItem.quantity_in ?? 0) - detail.quantity;
            materialItem.total_amount = newBalance * (materialItem.unit_price ?? detail.unit_price);
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);

            // ปิดรายการรับเข้า
            detail.is_active = false;
            detail.updated_at = DateTime.UtcNow;
            unit.Repository<MaterialReceiveDetail>().Update(detail);

            // ปิด StockCard IN เดิม
            var receiveStockCards = await unit.Repository<MaterialStockCard>()
             .ListAsync(new MaterialStockCardByReceiveDetailForDeleteSpec(detail.receive_detail_id));

            foreach (var stockCard in receiveStockCards)
            {
                stockCard.is_active = false;
                stockCard.updated_at = DateTime.UtcNow;
                unit.Repository<MaterialStockCard>().Update(stockCard);
            }

            // เพิ่ม StockCard กลับรายการ
            var cancelStockCard = new MaterialStockCard
            {
                material_item_id = detail.material_item_id,
                transaction_date = DateTime.UtcNow,
                transaction_type = "CANCEL_IN",
                reference_document_no = procurementRecord.document_no,
                procurement_record_id = procurementRecord.procurement_record_id,
                receive_detail_id = detail.receive_detail_id,
                issue_detail_id = null,
                quantity_in = 0,
                quantity_out = detail.quantity,
                balance_qty = newBalance,
                unit_price = detail.unit_price,
                total_amount = newBalance * detail.unit_price,
                is_active = true,
                created_at = DateTime.UtcNow,
                fiscal_year_id = procurementRecord.fiscal_year_id,

            };

            unit.Repository<MaterialStockCard>().Add(cancelStockCard);
        }

        procurementRecord.is_active = false;
        procurementRecord.updated_at = DateTime.UtcNow;

        unit.Repository<Procurement_records>().Update(procurementRecord);

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem deleting procurement record");
    }

    [HttpPost("create-with-assets")]
    public async Task<ActionResult> CreateProcurementWithAssets(ProcurementAssetFullCreateDto dto)
    {
        if (dto.asset_items == null || dto.asset_items.Count == 0)
            return BadRequest("กรุณาเพิ่มข้อมูลครุภัณฑ์หลักอย่างน้อย 1 รายการ");

        foreach (var assetDto in dto.asset_items)
        {
            if (assetDto.asset_item == null)
                return BadRequest("กรุณาเพิ่มข้อมูลครุภัณฑ์หลัก");

            if (assetDto.asset_sub_items == null || assetDto.asset_sub_items.Count == 0)
                return BadRequest("กรุณาเพิ่มรายการครุภัณฑ์ย่อยอย่างน้อย 1 รายการ");
        }

        var totalAmount = dto.asset_items
            .SelectMany(x => x.asset_sub_items)
            .Sum(x => x.quantity * (x.unit_price ?? 0));

        var procurement = mapper.Map<Procurement_records>(dto.procurement_record);

        procurement.total_amount = totalAmount;
        procurement.amount_text = ThaiBahtTextConverter.ToThaiBahtText(totalAmount);
        procurement.is_active = true;
        procurement.created_at = DateTime.UtcNow;
        procurement.approval_date = procurement.status == "ดำเนินการแล้ว"
            ? DateTime.UtcNow
            : null;

        unit.Repository<Procurement_records>().Add(procurement);

        if (!await unit.Complete())
            return BadRequest("Problem creating procurement record");

        var createdAssetIds = new List<int>();

        foreach (var assetDto in dto.asset_items)
        {
            var assetItem = mapper.Map<AssetItem>(assetDto.asset_item);

            assetItem.procurement_record_id = procurement.procurement_record_id;
            assetItem.is_active = true;
            assetItem.created_at = DateTime.UtcNow;

            unit.Repository<AssetItem>().Add(assetItem);

            if (!await unit.Complete())
                return BadRequest("Problem creating asset item");

            createdAssetIds.Add(assetItem.asset_id);

            var itemNo = 1;

            foreach (var subDto in assetDto.asset_sub_items)
            {
                var subItem = mapper.Map<AssetSubItem>(subDto);

                subItem.asset_id = assetItem.asset_id;
                subItem.item_no = itemNo++;
                subItem.total_price = subItem.quantity * (subItem.unit_price ?? 0);
                subItem.is_active = true;
                subItem.created_at = DateTime.UtcNow;

                unit.Repository<AssetSubItem>().Add(subItem);
            }

            if (!await unit.Complete())
                return BadRequest("Problem creating asset sub items");
        }

        return Ok(new
        {
            procurement_record_id = procurement.procurement_record_id,
            asset_ids = createdAssetIds,
            total_amount = procurement.total_amount,
            amount_text = procurement.amount_text,
            message = "Create procurement with multiple assets successfully"
        });
    }

    [HttpPost("create-with-hire")]
    public async Task<ActionResult> CreateProcurementWithHire(
     [FromBody] ProcurementHireFullCreateDto dto
 )
    {
        if (dto.hire_details == null || dto.hire_details.Count == 0)
            return BadRequest("กรุณาเพิ่มรายการจัดจ้างอย่างน้อย 1 รายการ");

        var totalAmount = dto.hire_details.Sum(x => x.quantity * x.unit_price);

        var procurement = mapper.Map<Procurement_records>(dto.procurement_record);

        procurement.total_amount = totalAmount;
        procurement.amount_text = ThaiBahtTextConverter.ToThaiBahtText(totalAmount);
        procurement.is_active = true;
        procurement.created_at = DateTime.UtcNow;
        procurement.approval_date = procurement.status == "ดำเนินการแล้ว"
            ? DateTime.UtcNow
            : null;

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
                total_amount = procurement.total_amount,
                amount_text = procurement.amount_text,
                message = "Create procurement with hire details successfully"
            });
        }

        return BadRequest("Problem creating hire details");
    }

    [HttpPost("create-with-materials")]
    public async Task<ActionResult> CreateProcurementWithMaterials(
    [FromBody] ProcurementMaterialFullCreateDto dto
)
    {
        if (dto.material_receive_details == null || dto.material_receive_details.Count == 0)
            return BadRequest("กรุณาเพิ่มรายการพัสดุอย่างน้อย 1 รายการ");

        var totalAmount = dto.material_receive_details.Sum(x => x.quantity * x.unit_price);

        var procurement = mapper.Map<Procurement_records>(dto.procurement_record);

        procurement.total_amount = totalAmount;
        procurement.is_active = true;
        procurement.amount_text = ThaiBahtTextConverter.ToThaiBahtText(procurement.total_amount);
        procurement.created_at = DateTime.UtcNow;
        procurement.approval_date = procurement.status == "ดำเนินการแล้ว"
            ? DateTime.UtcNow
            : null;


        unit.Repository<Procurement_records>().Add(procurement);

        if (!await unit.Complete())
            return BadRequest("Problem creating procurement record");

        var itemNo = 1;

        foreach (var detailDto in dto.material_receive_details)
        {
            var materialReceiveDetail = mapper.Map<MaterialReceiveDetail>(detailDto);

            materialReceiveDetail.procurement_record_id = procurement.procurement_record_id;
            materialReceiveDetail.item_no = itemNo++;
            materialReceiveDetail.is_active = true;
            materialReceiveDetail.total_amount =
                materialReceiveDetail.quantity * materialReceiveDetail.unit_price;

            unit.Repository<MaterialReceiveDetail>().Add(materialReceiveDetail);

            if (!await unit.Complete())
                return BadRequest("Problem creating material receive detail");

            var materialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(materialReceiveDetail.material_item_id);

            if (materialItem == null)
                return BadRequest($"Material item not found: {materialReceiveDetail.material_item_id}");

            var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
            var oldBalance = stockCards
                .Where(x =>
                    x.is_active &&
                    x.material_item_id == materialReceiveDetail.material_item_id &&
                    x.department_id == procurement.department_id
                )
                .OrderByDescending(x => x.transaction_date)
                .ThenByDescending(x => x.stock_card_id)
                .Select(x => x.balance_qty)
                .FirstOrDefault();
            var newBalance = oldBalance + materialReceiveDetail.quantity;

            materialItem.quantity_in = (materialItem.quantity_in ?? 0) + materialReceiveDetail.quantity;
            materialItem.unit_price = materialReceiveDetail.unit_price;
            materialItem.total_amount = newBalance * materialReceiveDetail.unit_price;
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);


            var stockCard = new MaterialStockCard
            {
                material_item_id = materialReceiveDetail.material_item_id,
                transaction_date = DateTime.UtcNow,
                transaction_type = "IN",
                reference_document_no = procurement.document_no,
                receive_detail_id = materialReceiveDetail.receive_detail_id,
                procurement_record_id = procurement.procurement_record_id,
                issue_detail_id = null,
                quantity_in = materialReceiveDetail.quantity,
                quantity_out = 0,
                balance_qty = newBalance,
                unit_price = materialReceiveDetail.unit_price,
                total_amount = newBalance * materialReceiveDetail.unit_price,
                fiscal_year_id = procurement.fiscal_year_id,
                is_active = true,
                created_at = DateTime.UtcNow,
                department_id = procurement.department_id,
            };

            unit.Repository<MaterialStockCard>().Add(stockCard);

            if (!await unit.Complete())
                return BadRequest("Problem creating stock transaction");
        }

        return Ok(new
        {
            procurement_record_id = procurement.procurement_record_id,
            message = "Create procurement with materials successfully"
        });
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
