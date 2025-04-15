using Result;
using System.Linq.Expressions;

namespace ModularKitchenDesigner.Domain.Interfaces.SimpleEntity
{
    public interface ISimpleEntityLoader<TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        Task<CollectionResult<TDto>> GetAll();
        Task<BaseResult<TDto>> GetByUniquePropertyAsync( Expression<Func<TEntity, bool>>? predicate = null );
    }
}