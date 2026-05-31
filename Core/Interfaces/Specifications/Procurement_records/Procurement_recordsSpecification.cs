using Core.Entities;
using Core.Interfaces.Specifications.Procurement_records;
using Core.Specifications;

public class Procurement_recordsSpecification : BaseSpecification<Procurement_records>
{
    public Procurement_recordsSpecification(Procurement_recordsSpecParams specParams)
  : base(x =>  x.is_active &&
       (
      string.IsNullOrEmpty(specParams.Search) ||
      x.document_no.ToLower().Contains(specParams.Search)))
    {
        AddInclude(x => x.fiscal_Years);
        AddInclude(x => x.operation_Types);
        AddInclude(x => x.expense_Types);
        AddInclude(x => x.departments);
        AddInclude(x => x.vendors);
        AddInclude(x => x.fund_Categories);
        AddInclude(x => x.budget_Sources);
        AddInclude(x => x.staffs);
        AddInclude(x => x.staffs.Prefixes);
        AddInclude(x => x.projects);

        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.procurement_record_id);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.procurement_record_id);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.created_at);
                break;

            default:
                AddOrderBy(x => x.created_at);
                break;
        }
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

