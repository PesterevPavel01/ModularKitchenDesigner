using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MySqlConnector;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
           where TEntity : class
    {

        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets the Async Exists record based on a predicate.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>>? selector = null,
            CancellationToken cancellationToken = default) =>
            selector is null
                ? await _dbSet.AnyAsync(cancellationToken)
                : await _dbSet.AnyAsync(selector, cancellationToken);

        /// <summary>
        /// Change entity state for patch method on web api.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// /// <param name="state">The entity state.</param>
        public void ChangeEntityState(TEntity entity, EntityState state) => _dbContext.Entry(entity).State = state;

        /// <summary>
        /// Находит сущность с заданными значениями первичного ключа. Если найдена, прикрепляется к контексту и возвращается. Если сущность не найдена, возвращается null.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
        public ValueTask<TEntity?> FindAsync(params object[] keyValues) => _dbSet.FindAsync(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{TEntity}"/> that represents the asynchronous find operation. The task result contains the found entity or null.</returns>
        public ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken) => _dbSet.FindAsync(keyValues, cancellationToken);

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(entity);
        }

        public async Task<List<TEntity>> CreateMultipleAsync(List<TEntity> entities)
        {
            if (entities.Count == 0)
                throw new ArgumentNullException("Entities not found");

            _dbContext.AddRange(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="trackingType"><c>NoTracking</c> to disable changing tracking; <c>Tracking</c> to enable changing tracking; <c>NoTrackingWithIdentityResolution</c> to disable changing tracking but identity resolving. Default to <c>NoTracking</c>.</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            TrackingType trackingType = TrackingType.Tracking,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {

            var query = trackingType switch
            {
                TrackingType.NoTracking => _dbSet.AsNoTracking(),
                TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
                TrackingType.Tracking => _dbSet,
                _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
            };

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }

            var entities = await query.ToListAsync();
            return entities;
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="trackingType"><c>NoTracking</c> to disable changing tracking; <c>Tracking</c> to enable changing tracking; <c>NoTrackingWithIdentityResolution</c> to disable changing tracking but identity resolving. Default to <c>NoTracking</c>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            TrackingType trackingType = TrackingType.NoTracking)
        {
            var query = trackingType switch
            {
                TrackingType.NoTracking => _dbSet.AsNoTracking(),
                TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
                TrackingType.Tracking => _dbSet,
                _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
            };

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
            if (include is not null)
            {
                query = include(query);
            }
            return await query.Select(selector).ToListAsync();
        }

        /// <summary>
        /// Взять все записи, сгруппировав их по нужным полям 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="group"></param>
        /// <param name="selector"></param>
        /// <param name="trackingType"><c>NoTracking</c> to disable changing tracking; <c>Tracking</c> to enable changing tracking; <c>NoTrackingWithIdentityResolution</c> to disable changing tracking but identity resolving. Default to <c>NoTracking</c>.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<List<TResult>> GetAllAsync<TResult, TKey>(
            Func<IQueryable<TEntity>, IQueryable<IGrouping<TKey, TEntity>>> group,
            Func<IQueryable<IGrouping<TKey, TEntity>>, IQueryable<TResult>> selector,
            TrackingType trackingType = TrackingType.NoTracking,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        )
        {
            var query = trackingType switch
            {
                TrackingType.NoTracking => _dbSet.AsNoTracking(),
                TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
                TrackingType.Tracking => _dbSet,
                _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
            };

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
            if (include is not null)
            {
                query = include(query);
            }

            var groupping = group(query).AsQueryable();
            return await selector(groupping).ToListAsync();
        }

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="ignoreQueryFilters">Ignore query filters</param>
        /// <param name="ignoreAutoIncludes">Ignore automatic includes</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool ignoreQueryFilters = false,
            bool ignoreAutoIncludes = false)
        {
            var query = _dbSet.AsNoTracking();

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (ignoreAutoIncludes)
            {
                query = query.IgnoreAutoIncludes();
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> ReloadAsync(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException("Entity is null");
            else
                await _dbContext.Entry(entity).ReloadAsync();

            return entity;
        }

        public async Task<bool> RemoveAllAsync()
        {
            var entityType = typeof(TEntity);

            var tableName = _dbContext.Model.FindEntityType(entityType).GetTableName();

            if (string.IsNullOrEmpty(tableName))
                throw new InvalidOperationException("Unable to determine the table name.");

            var sqlString = "DELETE FROM `TransitOrder`";
            await _dbContext.Database.ExecuteSqlRawAsync(sqlString);

            return await Task.FromResult(true);
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }

        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(entity);
        }

        public async Task<List<TEntity>> UpdateMultipleAsync(List<TEntity> entities)
        {
            if (entities.Count == 0)
                throw new ArgumentNullException("Entities not found");

            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<List<TEntity>> CallProcedureAsync(string procedureName, params MySqlParameter[] parameters)
        {
            var commandText = $"CALL {procedureName}({string.Join(", ", parameters.Select(p => $"@{p.ParameterName}"))})";

            var entities = await _dbSet
                .FromSqlRaw(commandText, parameters)
                .ToListAsync();

            if (entities.Count == 0)
                throw new ArgumentException("No entity returned");

            return entities;
        }

        public async Task<TEntity> ExecuteSqlScript(string sqlScript)
        {
            var entities = await _dbSet.FromSqlRaw(sqlScript).ToListAsync();
            if (entities.Count == 0)
                throw new ArgumentNullException("No entity returned");
            return entities.FirstOrDefault();
        }
    }
}
