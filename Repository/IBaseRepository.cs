using Microsoft.EntityFrameworkCore.Query;
using MySqlConnector;
using System.Linq.Expressions;

namespace Repository
{
    public interface IBaseRepository<TEntity> 
        where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            TrackingType trackingType = TrackingType.NoTracking,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            TrackingType trackingType = TrackingType.NoTracking);


        Task<List<TResult>> GetAllAsync<TResult, TKey>(
            Func<IQueryable<TEntity>, IQueryable<IGrouping<TKey, TEntity>>> group,
            Func<IQueryable<IGrouping<TKey, TEntity>>, IQueryable<TResult>> selector,
            TrackingType trackingType = TrackingType.NoTracking,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<List<TEntity>> CreateMultipleAsync(List<TEntity> entitys);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> RemoveAsync(TEntity entity);

        Task<bool> RemoveAllAsync();

        Task<TEntity> ReloadAsync(TEntity entity);

        Task<bool> SaveChangesAsync();

        Task<List<TEntity>> UpdateMultipleAsync(List<TEntity> entitys);

        Task<TEntity> ExecuteSqlScript(string procedureName);

        Task<List<TEntity>> CallProcedureAsync(string procedureName, params MySqlParameter[] parameters);
    }
}
