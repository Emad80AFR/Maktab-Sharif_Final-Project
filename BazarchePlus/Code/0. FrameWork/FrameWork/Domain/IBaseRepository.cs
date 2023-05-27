using System.Linq.Expressions;

namespace FrameWork.Domain;

public interface IBaseRepository<in TKey,T> where T:BaseClass<TKey>
{
    Task<T?> Get(TKey id, CancellationToken cancellationToken);
    Task<List<T>> Get(CancellationToken cancellationToken);
    Task Create(T entity, CancellationToken cancellationToken);
    Task<bool> Exist(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);

}




