
using Core.Entities;
using Core.Interfaces.Specifications.AcquisitionMethod;
using Core.Specifications;

public class AcquisitionMethodSpecification : BaseSpecification<AcquisitionMethod>
{
    public AcquisitionMethodSpecification(AcquisitionMethodSpecParams specParams)
  : base(x => x.is_active &&
      (string.IsNullOrEmpty(specParams.Search) ||
      x.acquisition_method_name.ToLower().Contains(specParams.Search.ToLower())))
    {
        // SORT
        switch (specParams.Sort)
        {
            case "nameAsc":
                AddOrderBy(x => x.acquisition_method_name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.acquisition_method_name);
                break;

            case "idDesc":
                AddOrderByDescending(x => x.acquisition_method_id);
                break;

            default:
                AddOrderBy(x => x.acquisition_method_id);
                break;
        }


        ApplyPaging(
            specParams.PageSize * (specParams.PageIndex - 1),
            specParams.PageSize
        );
    }
}
    

