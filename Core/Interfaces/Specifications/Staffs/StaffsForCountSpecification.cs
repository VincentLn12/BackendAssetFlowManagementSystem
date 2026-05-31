using Core.Entities;
using Core.Interfaces.Specifications.Staffs;
using Core.Specifications;

public class StaffsForCountSpecification : BaseSpecification<Staffs>
{
    public StaffsForCountSpecification(StaffsSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.first_name.ToLower().Contains(specParams.Search) ||
                x.last_name.ToLower().Contains(specParams.Search)
            ))
    {
    }
}