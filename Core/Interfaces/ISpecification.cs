using System.Linq.Expressions;

namespace Core.Interfaces;

//1.ISpecification<T> – อินเทอร์เฟซหลักที่กำหนดสิ่งที่ spec ต้องมี เช่น Criteria, Includes
public interface ISpecification<T>
{
    //Expression<Func<input, output>>
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }
}

//Projection (การแปลงผลลัพธ์จาก Entity T ไปเป็นประเภทผลลัพธ์ TResult) เช่น DTO หรือ ViewModel โดยตรงจาก Database
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}