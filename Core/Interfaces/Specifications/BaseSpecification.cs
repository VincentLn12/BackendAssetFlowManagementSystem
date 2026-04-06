using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specifications;

//2.BaseSpecification<T> – คลาสพื้นฐานที่ implement ISpecification
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? Criteria => criteria;
}

