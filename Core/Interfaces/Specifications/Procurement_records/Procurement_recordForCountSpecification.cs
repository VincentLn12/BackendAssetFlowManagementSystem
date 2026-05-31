using Core.Entities;
using Core.Interfaces.Specifications.Procurement_records;
using Core.Specifications;

public class Procurement_recordForCountSpecification : BaseSpecification<Procurement_records>
{
    public Procurement_recordForCountSpecification(Procurement_recordsSpecParams specParams)
        : base(x =>
            x.is_active &&
            (
              string.IsNullOrEmpty(specParams.Search) ||
      x.document_no.ToLower().Contains(specParams.Search)
               
            ))
    {
    }
}