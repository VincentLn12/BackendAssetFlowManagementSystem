namespace API.Controllers;

public class PublicPortalController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet("staffs")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalStaffLookupDto>>> GetStaffs(
        [FromQuery] string? search
    )
    {
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var prefixes = await unit.Repository<Prefixes>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();

        var activeProcurements = procurements.Where(x => x.is_active).ToList();
        var activeMaterialWithdrawals = materialWithdrawals.Where(x => x.is_active).ToList();
        var activeMaterialIssues = materialIssues.Where(x => x.is_active).ToList();
        var activeMaterialStockOuts = stockCards
            .Where(x => x.is_active && IsStockOut(x) && x.quantity_out > 0)
            .ToList();
        var activeAssetWithdrawals = assetWithdrawals.Where(x => x.is_active).ToList();

        var materialIssueStaffIds = activeMaterialStockOuts
            .Where(x => x.issue_detail_id.HasValue)
            .Select(x => activeMaterialIssues.FirstOrDefault(issue => issue.issue_detail_id == x.issue_detail_id)?.staff_id)
            .Where(x => x.HasValue)
            .Select(x => x!.Value);

        var withdrawerIds = activeMaterialWithdrawals
            .Select(x => x.staff_id)
            .Concat(materialIssueStaffIds)
            .Concat(activeAssetWithdrawals.Select(x => x.staff_id))
            .Distinct()
            .ToHashSet();

        var data = staffs
            .Where(x => x.is_active && withdrawerIds.Contains(x.staff_id))
            .Select(staff =>
            {
                var prefix = prefixes.FirstOrDefault(x => x.prefix_id == staff.prefix_id);
                var department = departments.FirstOrDefault(x => x.department_id == staff.department_id);

                var materialProcurementIds = activeMaterialWithdrawals
                    .Where(x => x.staff_id == staff.staff_id)
                    .Select(x => x.procurement_record_id);

                var assetProcurementIds = activeAssetWithdrawals
                    .Where(x => x.staff_id == staff.staff_id)
                    .Select(x => x.procurement_record_id);

                var projectCount = activeProcurements
                    .Where(x => materialProcurementIds.Contains(x.procurement_record_id) || assetProcurementIds.Contains(x.procurement_record_id))
                    .Select(x => x.project_id)
                    .Distinct()
                    .Count();

                return new PublicPortalStaffLookupDto
                {
                    staff_id = staff.staff_id,
                    full_name = $"{prefix?.prefix_name ?? ""}{staff.first_name} {staff.last_name}".Trim(),
                    department_name = department?.department_name,
                    project_count = projectCount,
                    material_withdrawal_count = activeMaterialWithdrawals.Count(x => x.staff_id == staff.staff_id) +
                        activeMaterialStockOuts.Count(x =>
                            x.issue_detail_id.HasValue &&
                            activeMaterialIssues.Any(issue =>
                                issue.issue_detail_id == x.issue_detail_id &&
                                issue.staff_id == staff.staff_id
                            )
                        ),
                    asset_withdrawal_count = activeAssetWithdrawals.Count(x => x.staff_id == staff.staff_id)
                };
            })
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.full_name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (x.department_name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
            )
            .OrderBy(x => x.full_name)
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/projects")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalStaffProjectDto>>> GetProjectsByStaff(
        int staffId
    )
    {
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();

        var materialProcurementIds = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var assetProcurementIds = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var relatedProcurements = procurements
            .Where(x =>
                x.is_active &&
                (materialProcurementIds.Contains(x.procurement_record_id) || assetProcurementIds.Contains(x.procurement_record_id))
            )
            .ToList();

        var data = projects
            .Where(x => x.is_active)
            .Select(project =>
            {
                var projectProcurements = relatedProcurements
                    .Where(x => x.project_id == project.project_id)
                    .ToList();

                return new PublicPortalStaffProjectDto
                {
                    project_id = project.project_id,
                    project_code = project.project_code,
                    project_name = project.project_name,
                    project_budget_amount = project.project_budget_amount,
                    procurement_count = projectProcurements.Count,
                    material_withdrawal_count = projectProcurements.Count(x => materialProcurementIds.Contains(x.procurement_record_id)),
                    asset_withdrawal_count = projectProcurements.Count(x => assetProcurementIds.Contains(x.procurement_record_id))
                };
            })
            .Where(x => x.procurement_count > 0)
            .OrderBy(x => x.project_code)
            .ThenBy(x => x.project_name)
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/fiscal-years")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalFiscalYearDto>>> GetStaffFiscalYears(
        int staffId
    )
    {
        var fiscalYears = await unit.Repository<Fiscal_years>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();

        var materialProcurementIds = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id);

        var assetProcurementIds = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id);

        var relatedProcurementIds = materialProcurementIds
            .Concat(assetProcurementIds)
            .Distinct()
            .ToHashSet();

        var relatedFiscalYearIds = procurements
            .Where(x => x.is_active && relatedProcurementIds.Contains(x.procurement_record_id))
            .Select(x => x.fiscal_year_id)
            .Concat(
                stockCards
                    .Where(stockCard =>
                        stockCard.is_active &&
                        IsStockOut(stockCard) &&
                        stockCard.quantity_out > 0 &&
                        stockCard.fiscal_year_id.HasValue &&
                        stockCard.issue_detail_id.HasValue &&
                        materialIssues.Any(issue =>
                            issue.is_active &&
                            issue.issue_detail_id == stockCard.issue_detail_id.Value &&
                            issue.staff_id == staffId
                        )
                    )
                    .Select(stockCard => stockCard.fiscal_year_id!.Value)
            )
            .Distinct()
            .ToHashSet();

        var data = fiscalYears
            .Where(x => x.is_active && relatedFiscalYearIds.Contains(x.fiscal_year_id))
            .OrderByDescending(x => x.fiscal_year)
            .Select(x => new PublicPortalFiscalYearDto
            {
                fiscal_year_id = x.fiscal_year_id,
                fiscal_year = x.fiscal_year,
                year_name = x.year_name
            })
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/summary")]
    public async Task<ActionResult<PublicPortalStaffSummaryDto>> GetStaffSummary(
        int staffId,
        [FromQuery] int? fiscalYearId
    )
    {
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var prefixes = await unit.Repository<Prefixes>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();

        var staff = staffs.FirstOrDefault(x => x.is_active && x.staff_id == staffId);
        if (staff == null)
        {
            return NotFound();
        }

        var prefix = prefixes.FirstOrDefault(x => x.prefix_id == staff.prefix_id);
        var department = departments.FirstOrDefault(x => x.department_id == staff.department_id);

        var materialProcurementIds = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var relatedAssetWithdrawals = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .ToList();

        var assetProcurementIds = relatedAssetWithdrawals
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var relatedProcurementIds = materialProcurementIds
            .Concat(assetProcurementIds)
            .Distinct()
            .ToHashSet();

        var relatedProcurements = procurements
            .Where(x =>
                x.is_active &&
                relatedProcurementIds.Contains(x.procurement_record_id) &&
                (!fiscalYearId.HasValue || x.fiscal_year_id == fiscalYearId.Value)
            )
            .ToList();

        var filteredProcurementIds = relatedProcurements
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var filteredMaterialWithdrawals = materialWithdrawals
            .Where(x =>
                x.is_active &&
                x.staff_id == staffId &&
                filteredProcurementIds.Contains(x.procurement_record_id)
            )
            .ToList();

        var materialStockOutCount = stockCards.Count(stockCard =>
            stockCard.is_active &&
            IsStockOut(stockCard) &&
            stockCard.quantity_out > 0 &&
            (!fiscalYearId.HasValue || stockCard.fiscal_year_id == fiscalYearId.Value) &&
            stockCard.issue_detail_id.HasValue &&
            materialIssues.Any(issue =>
                issue.is_active &&
                issue.issue_detail_id == stockCard.issue_detail_id.Value &&
                issue.staff_id == staffId
            )
        );

        var filteredAssetWithdrawals = relatedAssetWithdrawals
            .Where(x => filteredProcurementIds.Contains(x.procurement_record_id))
            .ToList();

        var assetHoldingCount = relatedProcurements
            .SelectMany(procurement =>
            {
                var procurementAssetIds = assetItems
                    .Where(x => x.is_active && x.procurement_record_id == procurement.procurement_record_id)
                    .Select(x => x.asset_id)
                    .ToHashSet();

                return assetSubItems.Where(x => x.is_active && procurementAssetIds.Contains(x.asset_id));
            })
            .Count();

        return Ok(new PublicPortalStaffSummaryDto
        {
            staff_id = staff.staff_id,
            full_name = $"{prefix?.prefix_name ?? ""}{staff.first_name} {staff.last_name}".Trim(),
            department_name = department?.department_name,
            procurement_count = relatedProcurements.Count,
            project_count = relatedProcurements.Select(x => x.project_id).Distinct().Count(),
            material_withdrawal_count = filteredMaterialWithdrawals.Count + materialStockOutCount,
            asset_withdrawal_count = filteredAssetWithdrawals.Count,
            asset_holding_count = assetHoldingCount
        });
    }

    [HttpGet("projects")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalProjectDto>>> GetProjects(
        [FromQuery] string? search
    )
    {
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();

        var activeProjects = projects.Where(x => x.is_active);
        var activeProcurements = procurements.Where(x => x.is_active).ToList();
        var activeMaterialWithdrawals = materialWithdrawals.Where(x => x.is_active).ToList();
        var activeAssetWithdrawals = assetWithdrawals.Where(x => x.is_active).ToList();

        var data = activeProjects
            .Select(project =>
            {
                var projectProcurementIds = activeProcurements
                    .Where(x => x.project_id == project.project_id)
                    .Select(x => x.procurement_record_id)
                    .ToHashSet();

                var withdrawerIds = activeMaterialWithdrawals
                    .Where(x => projectProcurementIds.Contains(x.procurement_record_id))
                    .Select(x => x.staff_id)
                    .Concat(
                        activeAssetWithdrawals
                            .Where(x => projectProcurementIds.Contains(x.procurement_record_id))
                            .Select(x => x.staff_id)
                    )
                    .Distinct()
                    .Count();

                return new PublicPortalProjectDto
                {
                    project_id = project.project_id,
                    project_code = project.project_code,
                    project_name = project.project_name,
                    project_budget_amount = project.project_budget_amount,
                    procurement_count = projectProcurementIds.Count,
                    withdrawer_count = withdrawerIds
                };
            })
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.project_code.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.project_name.Contains(search, StringComparison.OrdinalIgnoreCase)
            )
            .OrderBy(x => x.project_code)
            .ThenBy(x => x.project_name)
            .ToList();

        return Ok(data);
    }

    [HttpGet("projects/{projectId:int}/withdrawers")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalWithdrawerDto>>> GetProjectWithdrawers(
        int projectId
    )
    {
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var prefixes = await unit.Repository<Prefixes>().ListAllAsync();

        if (!projects.Any(x => x.project_id == projectId && x.is_active))
        {
            return NotFound();
        }

        var projectProcurementIds = procurements
            .Where(x => x.is_active && x.project_id == projectId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var materialCounts = materialWithdrawals
            .Where(x => x.is_active && projectProcurementIds.Contains(x.procurement_record_id))
            .GroupBy(x => x.staff_id)
            .ToDictionary(x => x.Key, x => x.Count());

        var assetCounts = assetWithdrawals
            .Where(x => x.is_active && projectProcurementIds.Contains(x.procurement_record_id))
            .GroupBy(x => x.staff_id)
            .ToDictionary(x => x.Key, x => x.Count());

        var withdrawerIds = materialCounts.Keys
            .Concat(assetCounts.Keys)
            .Distinct()
            .ToHashSet();

        var data = staffs
            .Where(x => x.is_active && withdrawerIds.Contains(x.staff_id))
            .Select(staff =>
            {
                var prefix = prefixes.FirstOrDefault(x => x.prefix_id == staff.prefix_id);
                var department = departments.FirstOrDefault(x => x.department_id == staff.department_id);

                return new PublicPortalWithdrawerDto
                {
                    staff_id = staff.staff_id,
                    full_name = $"{prefix?.prefix_name ?? ""}{staff.first_name} {staff.last_name}".Trim(),
                    department_name = department?.department_name,
                    material_withdrawal_count = materialCounts.GetValueOrDefault(staff.staff_id, 0),
                    asset_withdrawal_count = assetCounts.GetValueOrDefault(staff.staff_id, 0)
                };
            })
            .OrderBy(x => x.full_name)
            .ToList();

        return Ok(data);
    }

    [HttpGet("projects/{projectId:int}/staffs/{staffId:int}/procurements")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalProcurementSummaryDto>>> GetStaffProcurementsInProject(
        int projectId,
        int staffId
    )
    {
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var expenseTypes = await unit.Repository<Expense_types>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var hireDetails = await unit.Repository<HireDetail>().ListAllAsync();

        if (!projects.Any(x => x.project_id == projectId && x.is_active))
        {
            return NotFound();
        }

        var materialProcurementIds = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var assetProcurementIds = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var relatedProcurementIds = materialProcurementIds
            .Concat(assetProcurementIds)
            .Distinct()
            .ToHashSet();

        var data = procurements
            .Where(x =>
                x.is_active &&
                x.project_id == projectId &&
                relatedProcurementIds.Contains(x.procurement_record_id)
            )
            .Select(procurement =>
            {
                var department = departments.FirstOrDefault(x => x.department_id == procurement.department_id);
                var expenseType = expenseTypes.FirstOrDefault(x => x.expense_type_id == procurement.expense_type_id);
                var expenseTypeName = expenseType?.expense_type_name ?? "-";

                var category = materialProcurementIds.Contains(procurement.procurement_record_id)
                    ? "วัสดุ"
                    : assetProcurementIds.Contains(procurement.procurement_record_id)
                        ? "ครุภัณฑ์"
                        : expenseTypeName;

                var procurementAssetItems = assetItems
                    .Where(x => x.is_active && x.procurement_record_id == procurement.procurement_record_id)
                    .ToList();

                var assetIds = procurementAssetItems.Select(x => x.asset_id).ToHashSet();

                return new PublicPortalProcurementSummaryDto
                {
                    procurement_record_id = procurement.procurement_record_id,
                    project_id = procurement.project_id,
                    project_code = projects.FirstOrDefault(x => x.project_id == procurement.project_id)?.project_code ?? string.Empty,
                    project_name = projects.FirstOrDefault(x => x.project_id == procurement.project_id)?.project_name ?? string.Empty,
                    document_no = procurement.document_no,
                    document_date = procurement.document_date,
                    status = procurement.status,
                    total_amount = procurement.total_amount,
                    department_name = department?.department_name,
                    expense_type_name = expenseTypeName,
                    category = category,
                    material_issue_count = materialIssues.Count(x =>
                        x.is_active &&
                        x.procurement_record_id == procurement.procurement_record_id &&
                        x.staff_id == staffId
                    ),
                    asset_item_count = procurementAssetItems.Count,
                    asset_sub_item_count = assetSubItems.Count(x => x.is_active && assetIds.Contains(x.asset_id)),
                    hire_detail_count = hireDetails.Count(x =>
                        x.is_active && x.procurement_record_id == procurement.procurement_record_id
                    )
                };
            })
            .OrderByDescending(x => x.document_date)
            .ThenByDescending(x => x.procurement_record_id)
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/procurements")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalProcurementSummaryDto>>> GetStaffProcurements(
        int staffId,
        [FromQuery] int? fiscalYearId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var expenseTypes = await unit.Repository<Expense_types>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var hireDetails = await unit.Repository<HireDetail>().ListAllAsync();

        var materialProcurementIds = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var assetProcurementIds = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .Select(x => x.procurement_record_id)
            .ToHashSet();

        var relatedProcurementIds = materialProcurementIds
            .Concat(assetProcurementIds)
            .Distinct()
            .ToHashSet();

        var data = procurements
            .Where(x =>
                x.is_active &&
                relatedProcurementIds.Contains(x.procurement_record_id) &&
                (!fiscalYearId.HasValue || x.fiscal_year_id == fiscalYearId.Value)
            )
            .Select(procurement =>
            {
                var department = departments.FirstOrDefault(x => x.department_id == procurement.department_id);
                var expenseType = expenseTypes.FirstOrDefault(x => x.expense_type_id == procurement.expense_type_id);
                var project = projects.FirstOrDefault(x => x.project_id == procurement.project_id);
                var expenseTypeName = expenseType?.expense_type_name ?? "-";

                var category = materialProcurementIds.Contains(procurement.procurement_record_id)
                    ? "วัสดุ"
                    : assetProcurementIds.Contains(procurement.procurement_record_id)
                        ? "ครุภัณฑ์"
                        : expenseTypeName;

                var procurementAssetItems = assetItems
                    .Where(x => x.is_active && x.procurement_record_id == procurement.procurement_record_id)
                    .ToList();

                var assetIds = procurementAssetItems.Select(x => x.asset_id).ToHashSet();

                return new PublicPortalProcurementSummaryDto
                {
                    procurement_record_id = procurement.procurement_record_id,
                    project_id = procurement.project_id,
                    project_code = project?.project_code ?? string.Empty,
                    project_name = project?.project_name ?? string.Empty,
                    document_no = procurement.document_no,
                    document_date = procurement.document_date,
                    status = procurement.status,
                    total_amount = procurement.total_amount,
                    department_name = department?.department_name,
                    expense_type_name = expenseTypeName,
                    category = category,
                    material_issue_count = materialIssues.Count(x =>
                        x.is_active &&
                        x.procurement_record_id == procurement.procurement_record_id &&
                        x.staff_id == staffId
                    ),
                    asset_item_count = procurementAssetItems.Count,
                    asset_sub_item_count = assetSubItems.Count(x => x.is_active && assetIds.Contains(x.asset_id)),
                    hire_detail_count = hireDetails.Count(x =>
                        x.is_active && x.procurement_record_id == procurement.procurement_record_id
                    )
                };
            })
            .OrderByDescending(x => x.document_date)
            .ThenByDescending(x => x.procurement_record_id)
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/assets")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalStaffAssetItemDto>>> GetStaffAssets(
        int staffId,
        [FromQuery] int? fiscalYearId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var assetHistories = await unit.Repository<AssetSubItemHistory>().ListAllAsync();
        var assetRepairs = await unit.Repository<AssetRepair>().ListAllAsync();

        var relatedAssetWithdrawals = assetWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .OrderByDescending(x => x.withdrawal_date)
            .ToList();

        var data = relatedAssetWithdrawals
            .SelectMany(withdrawal =>
            {
                var procurement = procurements.FirstOrDefault(x =>
                    x.is_active &&
                    x.procurement_record_id == withdrawal.procurement_record_id &&
                    (!fiscalYearId.HasValue || x.fiscal_year_id == fiscalYearId.Value)
                );

                if (procurement == null)
                {
                    return Enumerable.Empty<PublicPortalStaffAssetItemDto>();
                }

                var project = projects.FirstOrDefault(x => x.project_id == procurement.project_id);
                var department = departments.FirstOrDefault(x => x.department_id == procurement.department_id);
                var procurementAssetItems = assetItems
                    .Where(x => x.is_active && x.procurement_record_id == procurement.procurement_record_id)
                    .ToList();

                return procurementAssetItems.Select(assetItem =>
                {
                    var assetLevelSubItems = assetSubItems
                        .Where(x =>
                            x.is_active &&
                            x.asset_id == assetItem.asset_id
                        )
                        .ToList();

                    var subItemCount = assetLevelSubItems.Count;
                    int? runningStartNo = assetLevelSubItems.Count == 0
                        ? null
                        : assetLevelSubItems.Min(x => x.running_start_no);
                    int? runningEndNo = assetLevelSubItems.Count == 0
                        ? null
                        : assetLevelSubItems.Max(x => x.running_end_no);
                    int? fiscalAssetYear = assetLevelSubItems.Count == 0
                        ? null
                        : assetLevelSubItems.FirstOrDefault()?.fiscal_asset_year;

                    return new PublicPortalStaffAssetItemDto
                    {
                        asset_id = assetItem.asset_id,
                        procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
                        procurement_record_id = procurement.procurement_record_id,
                        project_id = procurement.project_id,
                        project_code = project?.project_code ?? string.Empty,
                        project_name = project?.project_name ?? string.Empty,
                        document_no = procurement.document_no,
                        document_date = procurement.document_date,
                        department_name = department?.department_name,
                        withdrawal_document_no = withdrawal.withdrawal_document_no,
                        withdrawal_date = withdrawal.withdrawal_date,
                        end_date = withdrawal.end_date,
                        end_reason = withdrawal.end_reason,
                        asset_name = assetItem.asset_name,
                        receive_date = assetItem.receive_date,
                        storage_location = withdrawal.storage_location,
                        purpose = withdrawal.purpose,
                        running_start_no = runningStartNo,
                        running_end_no = runningEndNo,
                        fiscal_asset_year = fiscalAssetYear,
                        sub_item_count = subItemCount,
                        history_count = assetHistories.Count(x =>
                            x.is_active &&
                            x.procurement_withdrawal_id == withdrawal.procurement_withdrawal_id
                        ),
                        repair_count = assetRepairs.Count(x =>
                            x.is_active &&
                            x.procurement_withdrawal_id == withdrawal.procurement_withdrawal_id
                        )
                    };
                });
            })
            .OrderByDescending(x => x.withdrawal_date)
            .ThenBy(x => x.asset_name)
            .ToList();

        return Ok(data);
    }

    [HttpGet("staffs/{staffId:int}/material-withdrawals")]
    public async Task<ActionResult<IReadOnlyList<PublicPortalMaterialWithdrawalHistoryDto>>> GetStaffMaterialWithdrawals(
        int staffId,
        [FromQuery] int? fiscalYearId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();
        var materialIssues = await unit.Repository<MaterialIssueDetail>().ListAllAsync();
        var stockCards = await unit.Repository<MaterialStockCard>().ListAllAsync();
        var materialItems = await unit.Repository<MaterialItem>().ListAllAsync();
        var materialUnits = await unit.Repository<MaterialUnit>().ListAllAsync();
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var prefixes = await unit.Repository<Prefixes>().ListAllAsync();

        var staff = staffs.FirstOrDefault(x => x.is_active && x.staff_id == staffId);
        if (staff == null)
        {
            return NotFound();
        }

        var prefix = prefixes.FirstOrDefault(x => x.prefix_id == staff.prefix_id);
        var staffNameKeys = new[]
            {
                $"{staff.first_name ?? ""} {staff.last_name ?? ""}",
                $"{prefix?.prefix_name ?? ""}{staff.first_name ?? ""} {staff.last_name ?? ""}"
            }
            .Select(NormalizePersonName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var withdrawalByProcurement = materialWithdrawals
            .Where(x => x.is_active && x.staff_id == staffId)
            .GroupBy(x => x.procurement_record_id)
            .ToDictionary(
                x => x.Key,
                x => x.OrderByDescending(w => w.material_withdrawal_id).First()
            );

        var issueById = materialIssues
            .Where(x => x.is_active && x.staff_id == staffId)
            .ToDictionary(x => x.issue_detail_id);

        var data = stockCards
            .Where(stockCard =>
                stockCard.is_active &&
                IsStockOut(stockCard) &&
                stockCard.quantity_out > 0 &&
                (
                    (stockCard.issue_detail_id.HasValue && issueById.ContainsKey(stockCard.issue_detail_id.Value)) ||
                    StaffNameMatches(stockCard.StaffName, staffNameKeys)
                )
            )
            .Select(stockCard =>
            {
                MaterialIssueDetail? issue = null;
                if (stockCard.issue_detail_id.HasValue)
                {
                    issueById.TryGetValue(stockCard.issue_detail_id.Value, out issue);
                }

                var procurement = procurements.FirstOrDefault(x =>
                    x.is_active &&
                    x.procurement_record_id == (stockCard.procurement_record_id ?? issue?.procurement_record_id ?? 0)
                );

                var rowFiscalYearId = stockCard.fiscal_year_id ?? procurement?.fiscal_year_id;
                if (fiscalYearId.HasValue && rowFiscalYearId != fiscalYearId.Value)
                {
                    return null;
                }

                var project = procurement == null
                    ? null
                    : projects.FirstOrDefault(x => x.project_id == procurement.project_id);
                var material = materialItems.FirstOrDefault(x => x.material_item_id == stockCard.material_item_id);
                var unitName = material == null
                    ? null
                    : materialUnits.FirstOrDefault(x => x.unit_id == material.unit_id)?.unit_name;
                MaterialWithdrawal? withdrawal = null;
                if (procurement != null)
                {
                    withdrawalByProcurement.TryGetValue(procurement.procurement_record_id, out withdrawal);
                }

                return new PublicPortalMaterialWithdrawalHistoryDto
                {
                    issue_detail_id = issue?.issue_detail_id ?? stockCard.issue_detail_id ?? stockCard.stock_card_id,
                    material_withdrawal_id = withdrawal?.material_withdrawal_id,
                    procurement_record_id = procurement?.procurement_record_id ?? 0,
                    material_item_id = stockCard.material_item_id,
                    project_id = procurement?.project_id ?? 0,
                    project_code = project?.project_code ?? string.Empty,
                    project_name = project?.project_name ?? string.Empty,
                    document_no = procurement?.document_no ?? stockCard.reference_document_no ?? string.Empty,
                    document_date = procurement?.document_date,
                    withdrawal_document_no = withdrawal?.withdrawal_document_no ?? stockCard.reference_document_no ?? string.Empty,
                    issue_date = stockCard.transaction_date,
                    material_code = material?.material_code ?? string.Empty,
                    material_name = material?.material_name ?? "-",
                    unit_name = unitName,
                    quantity = stockCard.quantity_out,
                    unit_price = stockCard.unit_price,
                    total_amount = stockCard.quantity_out * stockCard.unit_price,
                    remark = issue?.remark
                };
            })
            .Where(x => x != null)
            .Select(x => x!)
            .OrderByDescending(x => x.issue_date)
            .ThenByDescending(x => x.issue_detail_id)
            .ToList();

        return Ok(data);
    }

    private static bool IsStockOut(MaterialStockCard stockCard)
    {
        return string.Equals(stockCard.transaction_type?.Trim(), "OUT", StringComparison.OrdinalIgnoreCase);
    }

    private static bool StaffNameMatches(string? stockCardStaffName, IReadOnlyCollection<string> staffNameKeys)
    {
        var normalizedStockCardStaffName = NormalizePersonName(stockCardStaffName);
        return !string.IsNullOrWhiteSpace(normalizedStockCardStaffName) &&
            staffNameKeys.Any(key =>
                normalizedStockCardStaffName.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                key.Contains(normalizedStockCardStaffName, StringComparison.OrdinalIgnoreCase)
            );
    }

    private static string NormalizePersonName(string? value)
    {
        return new string((value ?? string.Empty).Where(x => !char.IsWhiteSpace(x)).ToArray());
    }

    [HttpGet("staffs/{staffId:int}/assets/{assetId:int}/withdrawals/{withdrawalId:int}")]
    public async Task<ActionResult<PublicPortalStaffAssetDetailDto>> GetStaffAssetDetail(
        int staffId,
        int assetId,
        int withdrawalId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var projects = await unit.Repository<Projects>().ListAllAsync();
        var departments = await unit.Repository<Departments>().ListAllAsync();
        var prefixes = await unit.Repository<Prefixes>().ListAllAsync();
        var staffs = await unit.Repository<Staffs>().ListAllAsync();
        var usageTypes = await unit.Repository<AssetUsageType>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var assetHistories = await unit.Repository<AssetSubItemHistory>().ListAllAsync();
        var assetRepairs = await unit.Repository<AssetRepair>().ListAllAsync();

        var withdrawal = assetWithdrawals.FirstOrDefault(x =>
            x.is_active &&
            x.staff_id == staffId &&
            x.procurement_withdrawal_id == withdrawalId
        );

        if (withdrawal == null)
        {
            return NotFound();
        }

        var assetItem = assetItems.FirstOrDefault(x =>
            x.is_active &&
            x.asset_id == assetId &&
            x.procurement_record_id == withdrawal.procurement_record_id
        );

        if (assetItem == null)
        {
            return NotFound();
        }

        var procurement = procurements.FirstOrDefault(x =>
            x.is_active &&
            x.procurement_record_id == withdrawal.procurement_record_id
        );

        if (procurement == null)
        {
            return NotFound();
        }

        var project = projects.FirstOrDefault(x => x.project_id == procurement.project_id);
        var department = departments.FirstOrDefault(x => x.department_id == procurement.department_id);

        var subItems = assetSubItems
            .Where(x => x.is_active && x.asset_id == assetItem.asset_id)
            .OrderBy(x => x.asset_sub_item_id)
            .Select(x => new PublicPortalAssetSubItemDto
            {
                asset_sub_item_id = x.asset_sub_item_id,
                sub_item_name = string.IsNullOrWhiteSpace(x.sub_item_name) ? "ครุภัณฑ์ย่อย" : x.sub_item_name,
                running_start_no = x.running_start_no,
                running_end_no = x.running_end_no,
                fiscal_asset_year = x.fiscal_asset_year,
                quantity = x.quantity,
                unit_price = x.unit_price,
                total_price = x.total_price,
                status = x.status,
                asset_code_end = assetItem != null &&
                    !string.IsNullOrWhiteSpace(assetItem.asset_code_prefix) &&
                    x.running_end_no > 0 &&
                    x.fiscal_asset_year > 0
                        ? $"{assetItem.asset_code_prefix}-{x.running_end_no.ToString("D4")}/{x.fiscal_asset_year}"
                        : string.Empty
            })
            .ToList();

        var histories = assetHistories
            .Where(x =>
                x.is_active &&
                x.procurement_withdrawal_id == withdrawal.procurement_withdrawal_id
            )
            .OrderByDescending(x => x.history_date)
            .ThenByDescending(x => x.sub_item_history_id)
            .Select(x =>
            {
                var usageType = usageTypes.FirstOrDefault(u => u.usage_type_id == x.usage_type_id);
                var staff = staffs.FirstOrDefault(s => s.staff_id == x.staff_id);
                var prefix = staff == null ? null : prefixes.FirstOrDefault(p => p.prefix_id == staff.prefix_id);

                return new PublicPortalAssetUsageHistoryDto
                {
                    sub_item_history_id = x.sub_item_history_id,
                    history_date = x.history_date,
                    history_type = x.history_type,
                    usage_type_name = usageType?.usage_type_name,
                    detail = x.detail,
                    full_name = staff == null
                        ? null
                        : $"{prefix?.prefix_name ?? ""}{staff.first_name} {staff.last_name}".Trim()
                };
            })
            .ToList();

        var repairs = assetRepairs
            .Where(x =>
                x.is_active &&
                x.procurement_withdrawal_id == withdrawal.procurement_withdrawal_id
            )
            .OrderByDescending(x => x.repair_date)
            .ThenByDescending(x => x.asset_repair_id)
            .Select(x =>
            {
                var staff = staffs.FirstOrDefault(s => s.staff_id == x.staff_id);
                var prefix = staff == null ? null : prefixes.FirstOrDefault(p => p.prefix_id == staff.prefix_id);

                return new PublicPortalAssetRepairDto
                {
                    asset_repair_id = x.asset_repair_id,
                    repair_document_no = x.repair_document_no,
                    repair_date = x.repair_date,
                    status = x.status,
                    problem_description = x.problem_description,
                    repair_description = x.repair_description,
                    repair_shop_name = x.repair_shop_name,
                    repair_cost = x.repair_cost,
                    decree_document_no = x.decree_document_no,
                    full_name = staff == null
                        ? null
                        : $"{prefix?.prefix_name ?? ""}{staff.first_name} {staff.last_name}".Trim()
                };
            })
            .ToList();

        return Ok(new PublicPortalStaffAssetDetailDto
        {
            asset_id = assetItem.asset_id,
            procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
            procurement_record_id = procurement.procurement_record_id,
            project_id = procurement.project_id,
            project_code = project?.project_code ?? string.Empty,
            project_name = project?.project_name ?? string.Empty,
            document_no = procurement.document_no,
            document_date = procurement.document_date,
            department_name = department?.department_name,
            withdrawal_document_no = withdrawal.withdrawal_document_no,
            withdrawal_date = withdrawal.withdrawal_date,
            end_date = withdrawal.end_date,
            end_reason = withdrawal.end_reason,
            asset_name = assetItem.asset_name,
            receive_date = assetItem.receive_date,
            storage_location = withdrawal.storage_location,
            purpose = withdrawal.purpose,
            running_start_no = subItems.Count == 0 ? null : subItems.Min(x => x.running_start_no),
            running_end_no = subItems.Count == 0 ? null : subItems.Max(x => x.running_end_no),
            fiscal_asset_year = subItems.Count == 0 ? null : subItems.First().fiscal_asset_year,
            sub_items = subItems,
            histories = histories,
            repairs = repairs
        });
    }

    [HttpGet("projects/{projectId:int}/staffs/{staffId:int}/procurements/{procurementId:int}")]
    public async Task<ActionResult<PublicPortalProcurementDetailDto>> GetProcurementDetail(
        int projectId,
        int staffId,
        int procurementId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var expenseTypes = await unit.Repository<Expense_types>().ListAllAsync();
        var materialReceiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAllAsync();
        var materialItems = await unit.Repository<MaterialItem>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var hireDetails = await unit.Repository<HireDetail>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();

        var procurement = procurements.FirstOrDefault(x =>
            x.is_active &&
            x.project_id == projectId &&
            x.procurement_record_id == procurementId
        );

        if (procurement == null)
        {
            return NotFound();
        }

        var hasMaterialWithdrawal = materialWithdrawals.Any(x =>
            x.is_active &&
            x.staff_id == staffId &&
            x.procurement_record_id == procurementId
        );

        var relatedAssetWithdrawals = assetWithdrawals
            .Where(x =>
                x.is_active &&
                x.staff_id == staffId &&
                x.procurement_record_id == procurementId
            )
            .OrderByDescending(x => x.withdrawal_date)
            .ToList();

        if (!hasMaterialWithdrawal && relatedAssetWithdrawals.Count == 0)
        {
            return NotFound();
        }

        var expenseTypeName = expenseTypes
            .FirstOrDefault(x => x.expense_type_id == procurement.expense_type_id)
            ?.expense_type_name ?? "-";

        var materialReceiveDetailDtos = materialReceiveDetails
            .Where(x =>
                x.is_active &&
                x.procurement_record_id == procurementId
            )
            .OrderBy(x => x.item_no)
            .ThenBy(x => x.receive_detail_id)
            .Select(receiveDetail =>
            {
                var material = materialItems.FirstOrDefault(x => x.material_item_id == receiveDetail.material_item_id);
                return new PublicPortalMaterialReceiveDetailDto
                {
                    receive_detail_id = receiveDetail.receive_detail_id,
                    item_no = receiveDetail.item_no,
                    procurement_record_id = receiveDetail.procurement_record_id,
                    material_item_id = receiveDetail.material_item_id,
                    material_name = material?.material_name ?? "-",
                    quantity = receiveDetail.quantity,
                    unit_price = receiveDetail.unit_price,
                    total_amount = receiveDetail.total_amount,
                    operation_reason = receiveDetail.operation_reason
                };
            })
            .ToList();

        var procurementAssetItems = assetItems
            .Where(x => x.is_active && x.procurement_record_id == procurementId)
            .ToList();

        var assetHistories = relatedAssetWithdrawals
            .SelectMany(withdrawal =>
                procurementAssetItems.SelectMany(assetItem =>
                {
                    var subItems = assetSubItems
                        .Where(x => x.is_active && x.asset_id == assetItem.asset_id)
                        .ToList();

                    if (subItems.Count == 0)
                    {
                        return
                        [
                            new PublicPortalAssetHistoryDto
                            {
                                procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
                                withdrawal_document_no = withdrawal.withdrawal_document_no,
                                withdrawal_date = withdrawal.withdrawal_date,
                                end_date = withdrawal.end_date,
                                end_reason = withdrawal.end_reason,
                                asset_name = assetItem.asset_name,
                                sub_item_name = "ครุภัณฑ์หลัก",
                                quantity = 1,
                                unit_price = null,
                                total_price = null,
                                storage_location = withdrawal.storage_location,
                                purpose = withdrawal.purpose
                            }
                        ];
                    }

                    return subItems.Select(subItem => new PublicPortalAssetHistoryDto
                    {
                        procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
                        withdrawal_document_no = withdrawal.withdrawal_document_no,
                        withdrawal_date = withdrawal.withdrawal_date,
                        end_date = withdrawal.end_date,
                        end_reason = withdrawal.end_reason,
                        asset_name = assetItem.asset_name,
                        sub_item_name = string.IsNullOrWhiteSpace(subItem.sub_item_name)
                            ? "ครุภัณฑ์ย่อย"
                            : subItem.sub_item_name,
                        quantity = subItem.quantity,
                        unit_price = subItem.unit_price,
                        total_price = subItem.total_price,
                        storage_location = withdrawal.storage_location,
                        purpose = withdrawal.purpose
                    });
                })
            )
            .ToList();

        var hireDetailDtos = hireDetails
            .Where(x => x.is_active && x.procurement_record_id == procurementId)
            .OrderBy(x => x.item_no)
            .Select(x => new PublicPortalHireDetailDto
            {
                hire_detail_id = x.hire_detail_id,
                item_no = x.item_no,
                hire_name = x.hire_name,
                quantity = x.quantity,
                unit_price = x.unit_price,
                total_amount = x.total_amount,
                operation_reason = x.operation_reason,
                remark = x.remark
            })
            .ToList();

        var category = hasMaterialWithdrawal
            ? "วัสดุ"
            : relatedAssetWithdrawals.Count > 0
                ? "ครุภัณฑ์"
                : expenseTypeName;

        return Ok(new PublicPortalProcurementDetailDto
        {
            procurement_record_id = procurement.procurement_record_id,
            document_no = procurement.document_no,
            document_date = procurement.document_date,
            status = procurement.status,
            total_amount = procurement.total_amount,
            expense_type_name = expenseTypeName,
            category = category,
            material_receive_details = materialReceiveDetailDtos,
            asset_histories = assetHistories,
            hire_details = hireDetailDtos
        });
    }

    [HttpGet("staffs/{staffId:int}/procurements/{procurementId:int}")]
    public async Task<ActionResult<PublicPortalProcurementDetailDto>> GetProcurementDetailByStaff(
        int staffId,
        int procurementId
    )
    {
        var procurements = await unit.Repository<Procurement_records>().ListAllAsync();
        var expenseTypes = await unit.Repository<Expense_types>().ListAllAsync();
        var materialReceiveDetails = await unit.Repository<MaterialReceiveDetail>().ListAllAsync();
        var materialItems = await unit.Repository<MaterialItem>().ListAllAsync();
        var assetWithdrawals = await unit.Repository<AssetWithdrawal>().ListAllAsync();
        var assetItems = await unit.Repository<AssetItem>().ListAllAsync();
        var assetSubItems = await unit.Repository<AssetSubItem>().ListAllAsync();
        var hireDetails = await unit.Repository<HireDetail>().ListAllAsync();
        var materialWithdrawals = await unit.Repository<MaterialWithdrawal>().ListAllAsync();

        var procurement = procurements.FirstOrDefault(x =>
            x.is_active &&
            x.procurement_record_id == procurementId
        );

        if (procurement == null)
        {
            return NotFound();
        }

        var hasMaterialWithdrawal = materialWithdrawals.Any(x =>
            x.is_active &&
            x.staff_id == staffId &&
            x.procurement_record_id == procurementId
        );

        var relatedAssetWithdrawals = assetWithdrawals
            .Where(x =>
                x.is_active &&
                x.staff_id == staffId &&
                x.procurement_record_id == procurementId
            )
            .OrderByDescending(x => x.withdrawal_date)
            .ToList();

        if (!hasMaterialWithdrawal && relatedAssetWithdrawals.Count == 0)
        {
            return NotFound();
        }

        var expenseTypeName = expenseTypes
            .FirstOrDefault(x => x.expense_type_id == procurement.expense_type_id)
            ?.expense_type_name ?? "-";

        var materialReceiveDetailDtos = materialReceiveDetails
            .Where(x =>
                x.is_active &&
                x.procurement_record_id == procurementId
            )
            .OrderBy(x => x.item_no)
            .ThenBy(x => x.receive_detail_id)
            .Select(receiveDetail =>
            {
                var material = materialItems.FirstOrDefault(x => x.material_item_id == receiveDetail.material_item_id);
                return new PublicPortalMaterialReceiveDetailDto
                {
                    receive_detail_id = receiveDetail.receive_detail_id,
                    item_no = receiveDetail.item_no,
                    procurement_record_id = receiveDetail.procurement_record_id,
                    material_item_id = receiveDetail.material_item_id,
                    material_name = material?.material_name ?? "-",
                    quantity = receiveDetail.quantity,
                    unit_price = receiveDetail.unit_price,
                    total_amount = receiveDetail.total_amount,
                    operation_reason = receiveDetail.operation_reason
                };
            })
            .ToList();

        var procurementAssetItems = assetItems
            .Where(x => x.is_active && x.procurement_record_id == procurementId)
            .ToList();

        var assetHistories = relatedAssetWithdrawals
            .SelectMany(withdrawal =>
                procurementAssetItems.SelectMany(assetItem =>
                {
                    var subItems = assetSubItems
                        .Where(x => x.is_active && x.asset_id == assetItem.asset_id)
                        .ToList();

                    if (subItems.Count == 0)
                    {
                        return
                        [
                            new PublicPortalAssetHistoryDto
                            {
                                procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
                                withdrawal_document_no = withdrawal.withdrawal_document_no,
                                withdrawal_date = withdrawal.withdrawal_date,
                                end_date = withdrawal.end_date,
                                end_reason = withdrawal.end_reason,
                                asset_name = assetItem.asset_name,
                                sub_item_name = "ครุภัณฑ์หลัก",
                                quantity = 1,
                                unit_price = null,
                                total_price = null,
                                storage_location = withdrawal.storage_location,
                                purpose = withdrawal.purpose
                            }
                        ];
                    }

                    return subItems.Select(subItem => new PublicPortalAssetHistoryDto
                    {
                        procurement_withdrawal_id = withdrawal.procurement_withdrawal_id,
                        withdrawal_document_no = withdrawal.withdrawal_document_no,
                        withdrawal_date = withdrawal.withdrawal_date,
                        end_date = withdrawal.end_date,
                        end_reason = withdrawal.end_reason,
                        asset_name = assetItem.asset_name,
                        sub_item_name = string.IsNullOrWhiteSpace(subItem.sub_item_name)
                            ? "ครุภัณฑ์ย่อย"
                            : subItem.sub_item_name,
                        quantity = subItem.quantity,
                        unit_price = subItem.unit_price,
                        total_price = subItem.total_price,
                        storage_location = withdrawal.storage_location,
                        purpose = withdrawal.purpose
                    });
                })
            )
            .ToList();

        var hireDetailDtos = hireDetails
            .Where(x => x.is_active && x.procurement_record_id == procurementId)
            .OrderBy(x => x.item_no)
            .Select(x => new PublicPortalHireDetailDto
            {
                hire_detail_id = x.hire_detail_id,
                item_no = x.item_no,
                hire_name = x.hire_name,
                quantity = x.quantity,
                unit_price = x.unit_price,
                total_amount = x.total_amount,
                operation_reason = x.operation_reason,
                remark = x.remark
            })
            .ToList();

        var category = hasMaterialWithdrawal
            ? "วัสดุ"
            : relatedAssetWithdrawals.Count > 0
                ? "ครุภัณฑ์"
                : expenseTypeName;

        return Ok(new PublicPortalProcurementDetailDto
        {
            procurement_record_id = procurement.procurement_record_id,
            document_no = procurement.document_no,
            document_date = procurement.document_date,
            status = procurement.status,
            total_amount = procurement.total_amount,
            expense_type_name = expenseTypeName,
            category = category,
            material_receive_details = materialReceiveDetailDtos,
            asset_histories = assetHistories,
            hire_details = hireDetailDtos
        });
    }
}
