
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
        if (dto.quantity <= 0)
            return BadRequest("จำนวนเบิกต้องมากกว่า 0");

        var departmentId = await GetDepartmentId(dto.procurement_record_id, dto.department_id);
        var currentBalance = await GetLatestBalance(dto.material_item_id, departmentId);

        if (currentBalance < dto.quantity)
            return BadRequest($"วัสดุคงเหลือไม่พอ คงเหลือ {currentBalance}");

        var materialIssueDetail = mapper.Map<MaterialIssueDetail>(dto);

        materialIssueDetail.is_active = true;
        materialIssueDetail.created_at = DateTime.UtcNow;
        materialIssueDetail.issue_date ??= DateTime.UtcNow;
        materialIssueDetail.unit_price = await GetLatestUnitPrice(dto.material_item_id, departmentId, dto.unit_price);
        materialIssueDetail.total_amount =
            materialIssueDetail.quantity * materialIssueDetail.unit_price;

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

        await RecalculateStockCards(materialIssueDetail.material_item_id, departmentId);
        await SyncMaterialItemFromStockCards(materialIssueDetail.material_item_id);

        if (!await unit.Complete())
            return BadRequest("Problem updating stock balances");

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

            var departmentId = await GetDepartmentId(itemDto.procurement_record_id, itemDto.department_id);
            var currentBalance = await GetLatestBalance(itemDto.material_item_id, departmentId);

            if (currentBalance < itemDto.quantity)
                return BadRequest(
                    $"วัสดุคงเหลือไม่พอ คงเหลือ {Convert.ToInt64(currentBalance)} ชิ้น"
                );

            var issueDetail = mapper.Map<MaterialIssueDetail>(itemDto);

            issueDetail.is_active = true;
            issueDetail.created_at = DateTime.UtcNow;
            issueDetail.issue_date ??= DateTime.UtcNow;
            issueDetail.unit_price = await GetLatestUnitPrice(itemDto.material_item_id, departmentId, itemDto.unit_price);
            issueDetail.total_amount = issueDetail.quantity * issueDetail.unit_price;

            unit.Repository<MaterialIssueDetail>().Add(issueDetail);

            if (!await unit.Complete())
                return BadRequest("Problem creating material issue detail");

            var newBalance = currentBalance - issueDetail.quantity;

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

            await RecalculateStockCards(issueDetail.material_item_id, departmentId);
            await SyncMaterialItemFromStockCards(issueDetail.material_item_id);

            if (!await unit.Complete())
                return BadRequest("Problem updating stock balances");
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
        var oldProcurementRecordId = existingIssue.procurement_record_id;
        var oldDepartmentId = await GetDepartmentId(existingIssue.procurement_record_id, null);
        var newDepartmentId = await GetDepartmentId(dto.procurement_record_id, dto.department_id);

        if (dto.quantity <= 0)
            return BadRequest("จำนวนเบิกต้องมากกว่า 0");

        var availableBalance = await GetLatestBalance(dto.material_item_id, newDepartmentId);
        if (oldMaterialItemId == dto.material_item_id && oldDepartmentId == newDepartmentId)
        {
            availableBalance += existingIssue.quantity;
        }

        if (availableBalance < dto.quantity)
            return BadRequest($"วัสดุคงเหลือไม่พอ คงเหลือ {availableBalance}");

        mapper.Map(dto, existingIssue);

        existingIssue.unit_price = await GetLatestUnitPrice(dto.material_item_id, newDepartmentId, dto.unit_price);
        existingIssue.total_amount = existingIssue.quantity * existingIssue.unit_price;
        existingIssue.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialIssueDetail>().Update(existingIssue);

        var issueDate = existingIssue.issue_date ?? DateTime.UtcNow;
        var fiscalYearId = await GetFiscalYearId(issueDate);

        if (!fiscalYearId.HasValue)
            return BadRequest("ไม่พบปีงบประมาณของวันที่เบิก");

        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var outStockCard = stockCards.FirstOrDefault(x =>
            x.is_active &&
            x.issue_detail_id == existingIssue.issue_detail_id &&
            x.transaction_type == "OUT"
        );

        if (outStockCard == null)
        {
            outStockCard = new MaterialStockCard
            {
                transaction_type = "OUT",
                created_at = DateTime.UtcNow,
                is_active = true
            };
            unit.Repository<MaterialStockCard>().Add(outStockCard);
        }

        outStockCard.material_item_id = existingIssue.material_item_id;
        outStockCard.procurement_record_id = existingIssue.procurement_record_id;
        outStockCard.department_id = newDepartmentId;
        outStockCard.transaction_date = issueDate;
        outStockCard.issue_detail_id = existingIssue.issue_detail_id;
        outStockCard.fiscal_year_id = fiscalYearId;
        outStockCard.quantity_in = 0;
        outStockCard.quantity_out = existingIssue.quantity;
        outStockCard.balance_qty = 0;
        outStockCard.unit_price = existingIssue.unit_price;
        outStockCard.total_amount = 0;
        outStockCard.updated_at = DateTime.UtcNow;

        if (!await unit.Complete())
            return BadRequest("Problem updating the material issue detail");

        await RecalculateStockCards(oldMaterialItemId, oldDepartmentId);
        if (oldMaterialItemId != existingIssue.material_item_id || oldDepartmentId != newDepartmentId)
        {
            await RecalculateStockCards(existingIssue.material_item_id, newDepartmentId);
        }

        await SyncMaterialItemFromStockCards(oldMaterialItemId);
        await SyncMaterialItemFromStockCards(existingIssue.material_item_id);

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating stock balances");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialIssueDetail(int id)
    {
        var issueDetail = await unit.Repository<MaterialIssueDetail>().GetByIdAsync(id);

        if (issueDetail == null)
            return NotFound();

        if (!issueDetail.is_active)
            return BadRequest("รายการนี้ถูกลบไปแล้ว");

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
            balance_qty = 0,
            unit_price = issueDetail.unit_price,
            total_amount = 0,

            is_active = true,
            created_at = DateTime.UtcNow
        };

        unit.Repository<MaterialStockCard>().Add(cancelStockCard);

        if (!await unit.Complete())
            return BadRequest("Problem deleting material issue detail");

        await RecalculateStockCards(issueDetail.material_item_id, departmentId);
        await SyncMaterialItemFromStockCards(issueDetail.material_item_id);

        if (await unit.Complete())
            return NoContent();

        return BadRequest("Problem updating stock balances");
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

    private async Task<decimal> GetLatestBalance(int materialItemId, int? departmentId)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        return stockCards
            .Where(x =>
                x.is_active &&
                x.material_item_id == materialItemId &&
                x.department_id == departmentId
            )
            .OrderByDescending(x => x.transaction_date)
            .ThenByDescending(x => x.stock_card_id)
            .Select(x => x.balance_qty)
            .FirstOrDefault();
    }

    private async Task<decimal> GetLatestUnitPrice(int materialItemId, int? departmentId, decimal fallbackUnitPrice)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        return stockCards
            .Where(x =>
                x.is_active &&
                x.material_item_id == materialItemId &&
                x.department_id == departmentId
            )
            .OrderByDescending(x => x.transaction_date)
            .ThenByDescending(x => x.stock_card_id)
            .Select(x => x.unit_price)
            .FirstOrDefault() switch
        {
            0 => fallbackUnitPrice,
            var price => price
        };
    }

    private async Task RecalculateStockCards(int materialItemId, int? departmentId)
    {
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();

        var itemStockCards = stockCards
            .Where(x =>
                x.is_active &&
                x.material_item_id == materialItemId &&
                x.department_id == departmentId
            )
            .OrderBy(x => x.transaction_date)
            .ThenBy(x => x.stock_card_id)
            .ToList();

        decimal runningBalance = 0;

        foreach (var stockCard in itemStockCards)
        {
            runningBalance += stockCard.quantity_in - stockCard.quantity_out;
            stockCard.balance_qty = runningBalance;
            stockCard.total_amount = runningBalance * stockCard.unit_price;
            stockCard.updated_at = DateTime.UtcNow;
            unit.Repository<MaterialStockCard>().Update(stockCard);
        }
    }

    private async Task SyncMaterialItemFromStockCards(int materialItemId)
    {
        var materialItem = await unit.Repository<MaterialItem>().GetByIdAsync(materialItemId);
        if (materialItem == null)
            return;

        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var itemStockCards = stockCards
            .Where(x => x.is_active && x.material_item_id == materialItemId)
            .OrderBy(x => x.transaction_date)
            .ThenBy(x => x.stock_card_id)
            .ToList();

        var latestStockCard = itemStockCards.LastOrDefault();
        var groupedBalances = itemStockCards
            .GroupBy(x => x.department_id)
            .Select(g => g
                .OrderByDescending(x => x.transaction_date)
                .ThenByDescending(x => x.stock_card_id)
                .First()
                .balance_qty)
            .ToList();

        materialItem.quantity_in = itemStockCards.Sum(x => x.quantity_in);
        materialItem.quantity_out = itemStockCards.Sum(x => x.quantity_out);
        materialItem.unit_price = latestStockCard?.unit_price ?? materialItem.unit_price;
        var currentBalance = groupedBalances.Sum();
        materialItem.total_amount = currentBalance * (materialItem.unit_price ?? 0);
        materialItem.updated_at = DateTime.UtcNow;

        unit.Repository<MaterialItem>().Update(materialItem);
    }
}
