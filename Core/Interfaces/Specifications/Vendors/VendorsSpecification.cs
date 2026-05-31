using Core.Entities;
using Core.Interfaces.Specifications.Vendors;
using Core.Specifications;

public class VendorsSpecification : BaseSpecification<Vendors>
{
    public VendorsSpecification(VendorsSpecParams specParams)
  : base(x => x.is_active &&   
    (
      string.IsNullOrEmpty(specParams.Search) ||
      x.vendor_name.ToLower().Contains(specParams.Search) ||
      x.contact_name.ToLower().Contains(specParams.Search) || x.tax_no.ToLower().Contains(specParams.Search)))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.contact_name);
                break;
            case "nameDesc":
                AddOrderByDescending(x => x.contact_name);
                break;
            case "idDesc":
                AddOrderByDescending(x => x.vendor_id);
                break;
            default:
                AddOrderBy(x => x.vendor_id);
                break;
        }
        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

