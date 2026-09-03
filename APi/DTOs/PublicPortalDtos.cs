namespace APi.DTOs;

public class PublicPortalProjectDto
{
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public decimal project_budget_amount { get; set; }
    public int procurement_count { get; set; }
    public int withdrawer_count { get; set; }
}

public class PublicPortalStaffLookupDto
{
    public int staff_id { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string? department_name { get; set; }
    public int project_count { get; set; }
    public int material_withdrawal_count { get; set; }
    public int asset_withdrawal_count { get; set; }
}

public class PublicPortalFiscalYearDto
{
    public int fiscal_year_id { get; set; }
    public int fiscal_year { get; set; }
    public string? year_name { get; set; }
}

public class PublicPortalStaffProjectDto
{
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public decimal project_budget_amount { get; set; }
    public int procurement_count { get; set; }
    public int material_withdrawal_count { get; set; }
    public int asset_withdrawal_count { get; set; }
}

public class PublicPortalStaffSummaryDto
{
    public int staff_id { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string? department_name { get; set; }
    public int procurement_count { get; set; }
    public int project_count { get; set; }
    public int material_withdrawal_count { get; set; }
    public int asset_withdrawal_count { get; set; }
    public int asset_holding_count { get; set; }
}

public class PublicPortalWithdrawerDto
{
    public int staff_id { get; set; }
    public string full_name { get; set; } = string.Empty;
    public string? department_name { get; set; }
    public int material_withdrawal_count { get; set; }
    public int asset_withdrawal_count { get; set; }
}

public class PublicPortalProcurementSummaryDto
{
    public int procurement_record_id { get; set; }
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public string document_no { get; set; } = string.Empty;
    public DateTime? document_date { get; set; }
    public string status { get; set; } = string.Empty;
    public decimal total_amount { get; set; }
    public string? department_name { get; set; }
    public string expense_type_name { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public int material_issue_count { get; set; }
    public int asset_item_count { get; set; }
    public int asset_sub_item_count { get; set; }
    public int hire_detail_count { get; set; }
}

public class PublicPortalStaffAssetItemDto
{
    public int asset_id { get; set; }
    public int procurement_withdrawal_id { get; set; }
    public int procurement_record_id { get; set; }
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public string document_no { get; set; } = string.Empty;
    public DateTime? document_date { get; set; }
    public string? department_name { get; set; }
    public string withdrawal_document_no { get; set; } = string.Empty;
    public DateTime withdrawal_date { get; set; }
    public DateTime? end_date { get; set; }
    public string? end_reason { get; set; }
    public string asset_name { get; set; } = string.Empty;
    public DateTime receive_date { get; set; }
    public string? storage_location { get; set; }
    public string? purpose { get; set; }
    public int? running_start_no { get; set; }
    public int? running_end_no { get; set; }
    public int? fiscal_asset_year { get; set; }
    public int sub_item_count { get; set; }
    public int history_count { get; set; }
    public int repair_count { get; set; }
}

public class PublicPortalMaterialWithdrawalHistoryDto
{
    public int issue_detail_id { get; set; }
    public int? material_withdrawal_id { get; set; }
    public int procurement_record_id { get; set; }
    public int material_item_id { get; set; }
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public string document_no { get; set; } = string.Empty;
    public DateTime? document_date { get; set; }
    public string withdrawal_document_no { get; set; } = string.Empty;
    public DateTime? issue_date { get; set; }
    public string material_code { get; set; } = string.Empty;
    public string material_name { get; set; } = string.Empty;
    public string? unit_name { get; set; }
    public decimal quantity { get; set; }
    public decimal unit_price { get; set; }
    public decimal? total_amount { get; set; }
    public string? remark { get; set; }
}

public class PublicPortalAssetSubItemDto
{
    public int asset_sub_item_id { get; set; }
    public string sub_item_name { get; set; } = string.Empty;
    public int running_start_no { get; set; }
    public int running_end_no { get; set; }
    public int fiscal_asset_year { get; set; }
    public decimal? quantity { get; set; }
    public decimal? unit_price { get; set; }
    public decimal? total_price { get; set; }
    public string? status { get; set; }
    public string? asset_code_end { get; set; }
}

public class PublicPortalAssetUsageHistoryDto
{
    public int sub_item_history_id { get; set; }
    public DateTime history_date { get; set; }
    public string history_type { get; set; } = string.Empty;
    public string? usage_type_name { get; set; }
    public string? detail { get; set; }
    public string? full_name { get; set; }
}

public class PublicPortalAssetRepairDto
{
    public int asset_repair_id { get; set; }
    public string repair_document_no { get; set; } = string.Empty;
    public DateTime repair_date { get; set; }
    public string status { get; set; } = string.Empty;
    public string? problem_description { get; set; }
    public string? repair_description { get; set; }
    public string? repair_shop_name { get; set; }
    public decimal? repair_cost { get; set; }
    public string? decree_document_no { get; set; }
    public string? full_name { get; set; }
}

public class PublicPortalStaffAssetDetailDto
{
    public int asset_id { get; set; }
    public int procurement_withdrawal_id { get; set; }
    public int procurement_record_id { get; set; }
    public int project_id { get; set; }
    public string project_code { get; set; } = string.Empty;
    public string project_name { get; set; } = string.Empty;
    public string document_no { get; set; } = string.Empty;
    public DateTime? document_date { get; set; }
    public string? department_name { get; set; }
    public string withdrawal_document_no { get; set; } = string.Empty;
    public DateTime withdrawal_date { get; set; }
    public DateTime? end_date { get; set; }
    public string? end_reason { get; set; }
    public string asset_name { get; set; } = string.Empty;
    public DateTime receive_date { get; set; }
    public string? storage_location { get; set; }
    public string? purpose { get; set; }
    public int? running_start_no { get; set; }
    public int? running_end_no { get; set; }
    public int? fiscal_asset_year { get; set; }
    public List<PublicPortalAssetSubItemDto> sub_items { get; set; } = [];
    public List<PublicPortalAssetUsageHistoryDto> histories { get; set; } = [];
    public List<PublicPortalAssetRepairDto> repairs { get; set; } = [];
}

public class PublicPortalProcurementDetailDto
{
    public int procurement_record_id { get; set; }
    public string document_no { get; set; } = string.Empty;
    public DateTime? document_date { get; set; }
    public string status { get; set; } = string.Empty;
    public decimal total_amount { get; set; }
    public string expense_type_name { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public List<PublicPortalMaterialReceiveDetailDto> material_receive_details { get; set; } = [];
    public List<PublicPortalAssetHistoryDto> asset_histories { get; set; } = [];
    public List<PublicPortalHireDetailDto> hire_details { get; set; } = [];
}

public class PublicPortalMaterialReceiveDetailDto
{
    public int receive_detail_id { get; set; }
    public int item_no { get; set; }
    public int procurement_record_id { get; set; }
    public int material_item_id { get; set; }
    public string material_name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal unit_price { get; set; }
    public decimal? total_amount { get; set; }
    public string? operation_reason { get; set; }
}

public class PublicPortalAssetHistoryDto
{
    public int procurement_withdrawal_id { get; set; }
    public string withdrawal_document_no { get; set; } = string.Empty;
    public DateTime withdrawal_date { get; set; }
    public DateTime? end_date { get; set; }
    public string? end_reason { get; set; }
    public string asset_name { get; set; } = string.Empty;
    public string sub_item_name { get; set; } = string.Empty;
    public decimal? quantity { get; set; }
    public decimal? unit_price { get; set; }
    public decimal? total_price { get; set; }
    public string? storage_location { get; set; }
    public string? purpose { get; set; }
}

public class PublicPortalHireDetailDto
{
    public int hire_detail_id { get; set; }
    public int item_no { get; set; }
    public string hire_name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal unit_price { get; set; }
    public decimal total_amount { get; set; }
    public string? operation_reason { get; set; }
    public string? remark { get; set; }
}
