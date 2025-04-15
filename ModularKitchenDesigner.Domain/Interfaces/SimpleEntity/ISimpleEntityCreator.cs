using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.SimpleEntity
{
    public interface ISimpleEntityCreator<TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        //IBaseRepository<TEntity> Repository { get; }
        Task<BaseResult<TDto>> CreateAsync(TDto model);
    }
}
