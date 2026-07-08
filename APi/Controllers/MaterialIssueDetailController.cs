
using Core.Interfaces.Specifications.MaterialIssueDetail;


namespace API.Controllers;

public class MaterialIssueDetailController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaterialIssueDetailDto>>> GetMaterialIssueDetails([FromQuery] MaterialIssueDetailSpecParams materialIssueDetailParams)
    {
        var spec = new MaterialIssueDetailSpecification(materialIssueDetailParams);

        var materialIssueDetails = await unit.Repository<MaterialIssueDetail>().ListAsync(spec);
         
        var countSpec = new MaterialIssueDetailSpecification(materialIssueDetailParams);    
        var totalItems = await unit.Repository<MaterialIssueDetail>()
            .CountAsync(countSpec);

        var data = mapper.Map<List<MaterialIssueDetailDto>>(materialIssueDetails);

        return Ok(new Pagination<MaterialIssueDetailDto>(
            materialIssueDetailParams.PageIndex,
            materialIssueDetailParams.PageSize,
            totalItems,
            data
        ));
    }
  
    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialIssueDetailDto>> GetMaterialIssueDetail(int id)
    {
        var materialIssueDetail = await unit.Repository<MaterialIssueDetail>().GetByIdAsync(id);

        if (materialIssueDetail == null) return NotFound();
        return mapper.Map<MaterialIssueDetailDto>(materialIssueDetail);
    }

    [HttpPost]
    public async Task<ActionResult<MaterialIssueDetailDto>> CreateMaterialIssueDetail(MaterialIssueDetailDto dto)
    {
        var materialItem = await unit.Repository<MaterialItem>()
            .GetByIdAsync(dto.material_item_id);

        if (materialItem == null)
            return BadRequest("ไม่พบข้อมูลวัสดุ");

        var currentBalance = materialItem.current_balance ?? 0;

        if (dto.quantity <= 0)
            return BadRequest("จำนวนเบิกต้องมากกว่า 0");

        if (currentBalance < dto.quantity)
            return BadRequest($"วัสดุคงเหลือไม่พอ คงเหลือ {currentBalance}");

        var materialIssueDetail = mapper.Map<MaterialIssueDetail>(dto);

        materialIssueDetail.is_active = true;
        materialIssueDetail.created_at = DateTime.UtcNow;
        materialIssueDetail.issue_date ??= DateTime.UtcNow;
        materialIssueDetail.unit_price = materialItem.unit_price ?? dto.unit_price;
        materialIssueDetail.total_amount =
            materialIssueDetail.quantity * materialIssueDetail.unit_price;
        var departmentId = await GetDepartmentId(materialIssueDetail.procurement_record_id, dto.department_id);

        string staffName = string.Empty;
        if (materialIssueDetail.staff_id.HasValue)
        {
            var staff = await unit.Repository<Staffs>()
                .GetByIdAsync(materialIssueDetail.staff_id.Value);

            if (staff != null)
            {
                staffName = $"{staff.first_name ?? ""} {staff.last_name ?? ""}".Trim();
            }
        }

        unit.Repository<MaterialIssueDetail>().Add(materialIssueDetail);

        if (!await unit.Complete())
            return BadRequest("Problem creating material issue detail");

        var newBalance = currentBalance - materialIssueDetail.quantity;

        materialItem.quantity_out = (materialItem.quantity_out ?? 0) + materialIssueDetail.quantity;
        materialItem.current_balance = newBalance;
        materialItem.total_amount = newBalance * materialIssueDetail.unit_price;
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);

        var issueDate = materialIssueDetail.issue_date ?? DateTime.UtcNow;
        var fiscalYearId = await GetFiscalYearId(issueDate);

        if (!fiscalYearId.HasValue)
            return BadRequest("ไม่พบปีงบประมาณของวันที่เบิก");

        var stockCard = new MaterialStockCard
        {
            material_item_id = materialIssueDetail.material_item_id,
            procurement_record_id = materialIssueDetail.procurement_record_id,
            department_id = departmentId,
            transaction_date = DateTime.UtcNow,
            transaction_type = "OUT",
            reference_document_no = null,
            receive_detail_id = null,
            issue_detail_id = materialIssueDetail.issue_detail_id,
            fiscal_year_id = fiscalYearId,
            StaffName = staffName,

            quantity_in = 0,
            quantity_out = materialIssueDetail.quantity,
            balance_qty = newBalance,
            unit_price = materialIssueDetail.unit_price,
            total_amount = newBalance * materialIssueDetail.unit_price,

            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(stockCard);

        if (!await unit.Complete())
            return BadRequest("Problem creating stock card");

        return CreatedAtAction(
            nameof(GetMaterialIssueDetail),
            new { id = materialIssueDetail.issue_detail_id },
            mapper.Map<MaterialIssueDetailDto>(materialIssueDetail)
        );
    }

    [HttpPost("create-many")]
    public async Task<ActionResult> CreateManyMaterialIssueDetails(
    [FromBody] MaterialIssueDetailManyCreateDto dto)
    {
        if (dto.items == null || dto.items.Count == 0)
            return BadRequest("กรุณาเพิ่มรายการเบิกอย่างน้อย 1 รายการ");

        foreach (var itemDto in dto.items)
        {
            if (itemDto.quantity <= 0)
                return BadRequest("จำนวนเบิกต้องมากกว่า 0");

            var materialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(itemDto.material_item_id);

            if (materialItem == null)
                return BadRequest($"ไม่พบวัสดุ ID: {itemDto.material_item_id}");

            var currentBalance = materialItem.current_balance ?? 0;

            if (currentBalance < itemDto.quantity)
                return BadRequest(
                    $"วัสดุคงเหลือไม่พอ คงเหลือ {Convert.ToInt64(currentBalance)} ชิ้น"
                );

            var issueDetail = mapper.Map<MaterialIssueDetail>(itemDto);

            issueDetail.is_active = true;
            issueDetail.created_at = DateTime.UtcNow;
            issueDetail.issue_date ??= DateTime.UtcNow;
            issueDetail.unit_price = materialItem.unit_price ?? itemDto.unit_price;
            issueDetail.total_amount = issueDetail.quantity * issueDetail.unit_price;
            var departmentId = await GetDepartmentId(issueDetail.procurement_record_id, itemDto.department_id);

            unit.Repository<MaterialIssueDetail>().Add(issueDetail);

            if (!await unit.Complete())
                return BadRequest("Problem creating material issue detail");

            var newBalance = currentBalance - issueDetail.quantity;

            materialItem.quantity_out = (materialItem.quantity_out ?? 0) + issueDetail.quantity;
            materialItem.current_balance = newBalance;
            materialItem.total_amount = newBalance * issueDetail.unit_price;
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);

            string staffName = string.Empty;

            if (issueDetail.staff_id.HasValue)
            {
                var staff = await unit.Repository<Staffs>()
                    .GetByIdAsync(issueDetail.staff_id.Value);

                if (staff != null)
                {
                    staffName = $"{staff.first_name ?? ""} {staff.last_name ?? ""}".Trim();
                }
            }

            var issueDate = issueDetail.issue_date ?? DateTime.UtcNow;
            var fiscalYearId = await GetFiscalYearId(issueDate);

            if (!fiscalYearId.HasValue)
                return BadRequest("ไม่พบปีงบประมาณของวันที่เบิก");

            var stockCard = new MaterialStockCard
            {
                material_item_id = issueDetail.material_item_id,
                procurement_record_id = issueDetail.procurement_record_id,
                department_id = departmentId,
                fiscal_year_id = fiscalYearId,

                transaction_date = DateTime.UtcNow,
                transaction_type = "OUT",
                receive_detail_id = null,
                issue_detail_id = issueDetail.issue_detail_id,

                quantity_in = 0,
                quantity_out = issueDetail.quantity,
                balance_qty = newBalance,
                unit_price = issueDetail.unit_price,
                total_amount = newBalance * issueDetail.unit_price,
                StaffName = staffName,

                is_active = true,
                created_at = DateTime.UtcNow
            };

            unit.Repository<MaterialStockCard>().Add(stockCard);

            if (!await unit.Complete())
                return BadRequest("Problem creating stock card");
        }

        return Ok(new
        {
            message = "Create many material issue details successfully",
            count = dto.items.Count
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialIssueDetail(int id, MaterialIssueDetailDto dto)
    {
        if (id != dto.issue_detail_id)
            return BadRequest("Cannot update this material issue detail");

        var existingIssue = await unit.Repository<MaterialIssueDetail>().GetByIdAsync(id);

        if (existingIssue == null)
            return NotFound("Material issue detail not found");

        if (!existingIssue.is_active)
            return BadRequest("ไม่สามารถแก้ไขรายการที่ถูกลบแล้ว");

        var oldMaterialItemId = existingIssue.material_item_id;
        var oldQuantity = existingIssue.quantity;
        var oldUnitPrice = existingIssue.unit_price;

        // กรณีเปลี่ยนวัสดุ
        if (oldMaterialItemId != dto.material_item_id)
        {
            var oldMaterialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(oldMaterialItemId);

            if (oldMaterialItem == null)
                return BadRequest("ไม่พบข้อมูลวัสดุเดิม");

            // คืน stock ให้ตัวเก่า
            oldMaterialItem.quantity_out = (oldMaterialItem.quantity_out ?? 0) - oldQuantity;
            oldMaterialItem.current_balance = (oldMaterialItem.current_balance ?? 0) + oldQuantity;
            oldMaterialItem.total_amount =
                (oldMaterialItem.current_balance ?? 0) * oldUnitPrice;
            oldMaterialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(oldMaterialItem);

            var newMaterialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(dto.material_item_id);

            if (newMaterialItem == null)
                return BadRequest("ไม่พบข้อมูลวัสดุใหม่");

            var newCurrentBalance = newMaterialItem.current_balance ?? 0;

            if (dto.quantity <= 0)
                return BadRequest("จำนวนเบิกต้องมากกว่า 0");

            if (newCurrentBalance < dto.quantity)
                return BadRequest($"วัสดุใหม่คงเหลือไม่พอ คงเหลือ {newCurrentBalance}");

            // ตัด stock ตัวใหม่
            newMaterialItem.quantity_out = (newMaterialItem.quantity_out ?? 0) + dto.quantity;
            newMaterialItem.current_balance = newCurrentBalance - dto.quantity;
            newMaterialItem.total_amount =
                (newMaterialItem.current_balance ?? 0) * (newMaterialItem.unit_price ?? dto.unit_price);
            newMaterialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(newMaterialItem);

            mapper.Map(dto, existingIssue);

            existingIssue.unit_price = newMaterialItem.unit_price ?? dto.unit_price;
            existingIssue.total_amount = existingIssue.quantity * existingIssue.unit_price;
            existingIssue.updated_at = DateTime.UtcNow;
            var departmentId = await GetDepartmentId(existingIssue.procurement_record_id, dto.department_id);

            var issueDate = existingIssue.issue_date ?? DateTime.UtcNow;
            var fiscalYearId = await GetFiscalYearId(issueDate);

            if (!fiscalYearId.HasValue)
                return BadRequest("ไม่พบปีงบประมาณของวันที่เบิก");
            var stockCard = new MaterialStockCard
            {
                material_item_id = dto.material_item_id,
                procurement_record_id = existingIssue.procurement_record_id,
                department_id = departmentId,
                transaction_date = DateTime.UtcNow,
                transaction_type = "UPDATE_OUT",
                issue_detail_id = existingIssue.issue_detail_id,

                quantity_in = 0,
                quantity_out = dto.quantity,
                balance_qty = newMaterialItem.current_balance ?? 0,
                unit_price = existingIssue.unit_price,
                total_amount = (newMaterialItem.current_balance ?? 0) * existingIssue.unit_price,

                is_active = true,
                created_at = DateTime.UtcNow
            };

            unit.Repository<MaterialStockCard>().Add(stockCard);
        }
        else
        {
            var materialItem = await unit.Repository<MaterialItem>()
                .GetByIdAsync(existingIssue.material_item_id);

            if (materialItem == null)
                return BadRequest("ไม่พบข้อมูลวัสดุ");

            if (dto.quantity <= 0)
                return BadRequest("จำนวนเบิกต้องมากกว่า 0");

            var currentBalance = materialItem.current_balance ?? 0;

            var diffQuantity = dto.quantity - oldQuantity;

            // ถ้าเพิ่มจำนวนเบิก ต้องเช็ค stock
            if (diffQuantity > 0 && currentBalance < diffQuantity)
                return BadRequest($"วัสดุคงเหลือไม่พอ คงเหลือ {currentBalance}");

            var newBalance = currentBalance - diffQuantity;

            materialItem.quantity_out = (materialItem.quantity_out ?? 0) + diffQuantity;
            materialItem.current_balance = newBalance;
            materialItem.total_amount = newBalance * (materialItem.unit_price ?? dto.unit_price);
            materialItem.updated_at = DateTime.UtcNow;

            unit.Repository<MaterialItem>().Update(materialItem);

            mapper.Map(dto, existingIssue);

            existingIssue.unit_price = materialItem.unit_price ?? dto.unit_price;
            existingIssue.total_amount = existingIssue.quantity * existingIssue.unit_price;
            existingIssue.updated_at = DateTime.UtcNow;
            var departmentId = await GetDepartmentId(existingIssue.procurement_record_id, dto.department_id);

            var issueDate = existingIssue.issue_date ?? DateTime.UtcNow;
            var fiscalYearId = await GetFiscalYearId(issueDate);

            if (!fiscalYearId.HasValue)
                return BadRequest("ไม่พบปีงบประมาณของวันที่เบิก");

            var stockCard = new MaterialStockCard
            {
                material_item_id = existingIssue.material_item_id,
                procurement_record_id = existingIssue.procurement_record_id,
                department_id = departmentId,
                fiscal_year_id = fiscalYearId,
                transaction_date = issueDate,
                transaction_type = "UPDATE_OUT",
                issue_detail_id = existingIssue.issue_detail_id,

                quantity_in = diffQuantity < 0 ? Math.Abs(diffQuantity) : 0,
                quantity_out = diffQuantity > 0 ? diffQuantity : 0,
                balance_qty = newBalance,
                unit_price = existingIssue.unit_price,
                total_amount = newBalance * existingIssue.unit_price,

                is_active = true,
                created_at = DateTime.UtcNow
            };

            unit.Repository<MaterialStockCard>().Add(stockCard);
        }

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating the material issue detail");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialIssueDetail(int id)
    {
        var issueDetail = await unit.Repository<MaterialIssueDetail>().GetByIdAsync(id);

        if (issueDetail == null)
            return NotFound();

        if (!issueDetail.is_active)
            return BadRequest("รายการนี้ถูกลบไปแล้ว");

        var materialItem = await unit.Repository<MaterialItem>()
            .GetByIdAsync(issueDetail.material_item_id);

        if (materialItem == null)
            return BadRequest("ไม่พบข้อมูลวัสดุ");

        var oldBalance = materialItem.current_balance ?? 0;
        var newBalance = oldBalance + issueDetail.quantity;

        materialItem.quantity_out = (materialItem.quantity_out ?? 0) - issueDetail.quantity;
        materialItem.current_balance = newBalance;
        materialItem.total_amount = newBalance * issueDetail.unit_price;
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);

        issueDetail.is_active = false;
        issueDetail.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialIssueDetail>().Update(issueDetail);
        var departmentId = await GetDepartmentId(issueDetail.procurement_record_id, null);

        var cancelStockCard = new MaterialStockCard
        {
            material_item_id = issueDetail.material_item_id,
            procurement_record_id = issueDetail.procurement_record_id,
            department_id = departmentId,
            transaction_date = DateTime.UtcNow,
            transaction_type = "CANCEL_OUT",
            issue_detail_id = issueDetail.issue_detail_id,

            quantity_in = issueDetail.quantity,
            quantity_out = 0,
            balance_qty = newBalance,
            unit_price = issueDetail.unit_price,
            total_amount = newBalance * issueDetail.unit_price,

            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(cancelStockCard);

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem deleting material issue detail");
    }

    private async Task<int?> GetFiscalYearId(DateTime issueDate)
    {
        var fiscalYears = await unit.Repository<Fiscal_years>()
            .ListAllAsync();

        var fiscalYear = fiscalYears.FirstOrDefault(x =>
            x.is_active &&
            issueDate.Date >= x.start_date.Date &&
            issueDate.Date <= x.end_date.Date
        );

        return fiscalYear?.fiscal_year_id;
    }

    private async Task<int?> GetDepartmentId(int? procurementRecordId, int? fallbackDepartmentId)
    {
        if (!procurementRecordId.HasValue)
            return fallbackDepartmentId;

        var procurementRecord = await unit.Repository<Procurement_records>()
            .GetByIdAsync(procurementRecordId.Value);

        return procurementRecord?.department_id ?? fallbackDepartmentId;
    }
}
