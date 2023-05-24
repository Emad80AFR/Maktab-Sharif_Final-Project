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

        public BaseRepository(DbContext context, ILogger<BaseRepository<TKey, T>> logger)
        {
            _logger = logger;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> Get(TKey id)
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

        public async Task<List<T>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all entities");
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all entities");
                throw;
            }
        }

        public async Task Create(T entity)
        {

            try
            {
                _logger.LogInformation("Creating entity");
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating entity");
                throw;
            }
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> expression)
        {
            try
            {
                _logger.LogInformation("Checking expression");
               return await _dbSet.AnyAsync(expression);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Checking expression");
                throw;
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                _logger.LogInformation("Saving data into database");
                await _dbSet.SingleAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Saving data into database");
                throw;
            }
        }
    }
}
