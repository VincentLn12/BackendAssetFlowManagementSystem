using APi.DTOs;
using AutoMapper;

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
                    s.staff != null
                        ? s.staff.first_name + " " + s.staff.last_name
                        : null
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
            //.ForMember(
            //    d => d.asset_category_name,
            //    o => o.MapFrom(s => s.AssetCategory!.category_name)
            //)
            //.ForMember(
            //    d => d.unit_name,
            //    o => o.MapFrom(s => s.Unit!.unit_name)
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
                d => d.staff_name,
                o => o.MapFrom(s => s.Staff != null ? $"{s.Staff.Prefixes!.prefix_name}{s.Staff.first_name} {s.Staff.last_name}" : null)
            )
            .ForMember(
                d => d.vendor_name,
                o => o.MapFrom(s => s.Vendor != null ? s.Vendor.vendor_name : null)
            );

        CreateMap<AssetItemCreateDto, AssetItem>();

    }
}