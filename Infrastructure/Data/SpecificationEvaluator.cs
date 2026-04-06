using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

//3.SpecificationEvaluator – คลาสที่เอา spec มาสร้าง query จริง (IQueryable<T>)
public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == "React"
        }

        return query;
    }

}
