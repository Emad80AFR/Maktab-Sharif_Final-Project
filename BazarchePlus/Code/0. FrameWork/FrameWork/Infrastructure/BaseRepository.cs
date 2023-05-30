using System.Linq.Expressions;
using FrameWork.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrameWork.Infrastructure
{
    public class BaseRepository<TKey, T> :IBaseRepository<TKey ,T> where T:BaseClass<TKey>
    { 
        private readonly ILogger<BaseRepository<TKey,T>> _logger;
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _context;

        public BaseRepository(DbContext context, ILogger<BaseRepository<TKey, T>> logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> Get(TKey id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting entity by ID: {Id}", id);
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting entity by ID: {Id}", id);
                throw;
            }
        }

        public async Task<List<T>> Get(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting all entities");
                return await _dbSet.ToListAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all entities");
                throw;
            }
        }

        public async Task Create(T entity, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Creating entity");
                await _dbSet.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating entity");
                throw;
            }
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Checking expression");
               return await _dbSet.AnyAsync(expression, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Checking expression");
                throw;
            }
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Saving data into database");
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Saving data into database");
                throw;
            }
        }
    }
}
