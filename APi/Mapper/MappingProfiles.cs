
namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        //บคลาการ
        CreateMap<Staffs, StaffDto>()
         .ForMember(
             d => d.full_name,
             o => o.MapFrom(s =>
                 $"{s.Prefixes!.prefix_name}{s.first_name} {s.last_name}"
             )
         )
         .ForMember(
             d => d.department_name,
             o => o.MapFrom(s => s.Departments!.department_name)
         )
         .ForMember(
             d => d.position_name,
             o => o.MapFrom(s => s.Positions!.position_name)    
         )
         .ForMember(
             d => d.prefix_name,
             o => o.MapFrom(s => s.Prefixes!.prefix_name)
         );

        CreateMap<StaffDto, Staffs>();

        //โครงงาน
        CreateMap<Projects, ProjectDto>()
            .ForMember(
                d => d.fiscal_year_name,
                o => o.MapFrom(s => s.fiscal_year!.year_name)
            )
            .ForMember(
                d => d.staff_name,
               o => o.MapFrom(s =>
                $"{s.staff!.Prefixes!.prefix_name}{s.staff.first_name} {s.staff.last_name}"
            )
            );
        CreateMap<ProjectAddUpdateDto, Projects>();

        //บันทึกการจัดซื้อจัดจ้าง
        CreateMap<Procurement_records, ProcurementRecordDto>()
        .ForMember(
            d => d.staff_fullname,
            o => o.MapFrom(s =>
                $"{s.staffs.Prefixes!.prefix_name}{s.staffs.first_name} {s.staffs.last_name}"
            )
        )
        .ForMember(
            d => d.fiscal_year_name,
            o => o.MapFrom(s => s.fiscal_Years!.fiscal_year)
        )
        .ForMember(
            d => d.operation_type_name,
            o => o.MapFrom(s => s.operation_Types!.operation_type_name)
        )
        .ForMember(
            d => d.expense_type_name,
            o => o.MapFrom(s => s.expense_Types!.expense_type_name)
        )
        .ForMember(
            d => d.department_name,
            o => o.MapFrom(s => s.departments!.department_name)
        )
        .ForMember(
            d => d.vendor_name,
            o => o.MapFrom(s => s.vendors!.vendor_name)
        )
        .ForMember(
            d => d.fund_category_name,
            o => o.MapFrom(s => s.fund_Categories!.fund_name)
        )
        .ForMember(
            d => d.budget_source_name,
            o => o.MapFrom(s => s.budget_Sources!.budget_source_name)
        )
        .ForMember(
            d => d.project_code,
            o => o.MapFrom(s => s.projects!.project_code)
        )
        ;
        CreateMap<ProcurementRecordCreateDto, Procurement_records>();

        CreateMap<HireDetail, HireDetailDto>()
            .ForMember(
            d => d.document_no,
            o => o.MapFrom(s =>
                $"{s.procurement_record!.document_no}"
            )
        ).ForMember(
            d => d.unit_name,
            o => o.MapFrom(s =>
                $"{s.unit!.unit_name}"

        ));

        CreateMap<HireDetailDto, HireDetail>();

        //ครุภัณฑ์
        CreateMap<AssetItem, AssetItemDto>()
            //)
            .ForMember(
                d => d.category_name,
                o => o.MapFrom(s => s.FundCategory != null ? s.FundCategory.fund_name : null)
            )
            .ForMember(
                d => d.department_name,
                o => o.MapFrom(s => s.Department != null ? s.Department.department_name : null)
            )         
             .ForMember(
                d => d.acquisition_method_name,
                o => o.MapFrom(s => s.AcquisitionMethod != null ? s.AcquisitionMethod.acquisition_method_name : null)
            );

        CreateMap<AssetItemCreateDto, AssetItem>();
           
        //คุรภัณฑ์ย่อย
        CreateMap<AssetSubItem, AssetSubItemDto>()
              .ForMember(
            d => d.asset_code_start,
            o => o.MapFrom(s =>
                s.assetItem != null &&
                !string.IsNullOrWhiteSpace(s.assetItem.asset_code_prefix) &&
                s.running_start_no > 0 &&
                s.fiscal_asset_year > 0
                    ? $"{s.assetItem.asset_code_prefix}-{s.running_start_no.ToString("D4")}/{s.fiscal_asset_year}"
                    : string.Empty
            )
        )
       .ForMember(
            d => d.quantity_with_unit,
            o => o.MapFrom(s =>
                $"{s.quantity:0.##} {s.materialUnit!.unit_name}"
            )
        )
       .ForMember(
    d => d.asset_code_end,
    o => o.MapFrom(s =>
        s.assetItem != null &&
        !string.IsNullOrWhiteSpace(s.assetItem.asset_code_prefix) &&
        s.running_end_no > 0 &&
        s.fiscal_asset_year > 0
            ? $"{s.assetItem.asset_code_prefix}-{s.running_end_no.ToString("D4")}/{s.fiscal_asset_year}"
            : string.Empty
    )
)
        .ForMember(
                d => d.category_name,
                o => o.MapFrom(s => s.asset_category != null ? s.asset_category.category_name : null)
            )

        .ForMember(
                d => d.unit_name,
                o => o.MapFrom(s => s.materialUnit != null ? s.materialUnit.unit_name : null)
            );
        CreateMap<AssetSubItemCreateDto, AssetSubItem>();

        CreateMap<AssetSubItemDisposal, AssetSubItemDisposalDto>();
        CreateMap<AssetSubItemDisposalDto, AssetSubItemDisposal>();

        //การซ่อมบำรุงครุภัณฑ์
        CreateMap<AssetRepair, AssetRepairDto>()
        .ForMember(
            d => d.FullName,
            o => o.MapFrom(s =>
                $"{s.Staff!.Prefixes!.prefix_name}{s.Staff.first_name} {s.Staff.last_name}"
            )
        );
        CreateMap<AssetRepairDto, AssetRepair>();

        //การเบิกครุภัณฑ์
        CreateMap<AssetWithdrawal, AssetWithdrawalDto>()
             .ForMember(
            d => d.staff_name,
            o => o.MapFrom(s =>
                $"{s.Staff.Prefixes!.prefix_name}{s.Staff.first_name} {s.Staff.last_name}"
            )
        );
        CreateMap<AssetWithdrawalCreateDto, AssetWithdrawal>();
        
        //ประวัติการใช้งานครุภัณฑ์
        CreateMap<AssetSubItemHistory, AssetSubItemHistoryDto>()
         .ForMember(
                d => d.usage_type_name,
                o => o.MapFrom(s => s.AssetUsageType != null ? s.AssetUsageType.usage_type_name : null)
            )
        .ForMember(
            d => d.FullName,
            o => o.MapFrom(s =>
                $"{s.Staff!.Prefixes!.prefix_name}{s.Staff.first_name} {s.Staff.last_name}"
            )
        );
        CreateMap<AssetSubItemHistoryDto, AssetSubItemHistory>();
        //การจ้างเหมาบริการ
        CreateMap<HireDetailCreateDto, HireDetail>();
        //วัสดุ
        CreateMap<MaterialItem, MaterialItemDto>()
             .ForMember(
                d => d.unit_name,
                o => o.MapFrom(s => s.Unit != null ? s.Unit.unit_name : null)
            );

        CreateMap<MaterialItemDto, MaterialItem>();
        //การเบิกวัสดุ
        CreateMap<MaterialIssueDetail, MaterialIssueDetailDto>()
            .ForMember(
            d => d.staff_fullname,
            o => o.MapFrom(s =>
                s.Requester != null
                    ? $"{s.Requester.Prefixes!.prefix_name}{s.Requester.first_name} {s.Requester.last_name}"
                    : null
            )
        );
        CreateMap<MaterialIssueDetailDto, MaterialIssueDetail>();

        //การรับวัสดุ
        CreateMap<MaterialReceiveDetail, MaterialReceiveDetailDto>()
            .ForMember(
            d => d.material_name,
            o => o.MapFrom(s =>
                s.MaterialItem != null
                    ? s.MaterialItem.material_name
                    : null
            )
        );
        CreateMap<MaterialReceiveDetailDto, MaterialReceiveDetail>() ;
        CreateMap<MaterialReceiveDetailCreateDto, MaterialReceiveDetail>();

        CreateMap<MaterialWithdrawal, MaterialWithdrawalDto>()
         .ForMember(
         d => d.staff_name,
         o => o.MapFrom(s =>
             s.staffs != null
                 ? $"{s.staffs.Prefixes!.prefix_name}{s.staffs.first_name} {s.staffs.last_name}"   
                 : null
         ));
        CreateMap<MaterialWithdrawalCreateDto, MaterialWithdrawal>();

    }
}