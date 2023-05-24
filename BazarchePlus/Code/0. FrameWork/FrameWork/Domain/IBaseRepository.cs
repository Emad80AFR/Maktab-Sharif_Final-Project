using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace FrameWork.Domain;

public interface IBaseRepository<in TKey,T> where T:BaseClass<TKey>
{
    Task<T?> Get(TKey id);
    Task<List<T>> Get();
    Task Create(T entity);
    Task<bool> Exist(Expression<Func<T, bool>> expression);
    Task SaveChanges();

}




