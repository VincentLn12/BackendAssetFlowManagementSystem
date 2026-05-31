using Core.Entities;
using Core.Interfaces.Specifications.HireDetail;
using Core.Specifications;

public class HireDetailForCountSpecification : BaseSpecification<HireDetail>
{
    public HireDetailForCountSpecification(HireDetailSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
              string.IsNullOrEmpty(specParams.Search) ||
      x.hire_name.ToLower().Contains(specParams.Search)

            ))
    {
    }
}