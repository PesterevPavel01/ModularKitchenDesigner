using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.SimpleEntity
{
    public interface ISimpleEntityUpdater <TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        //IBaseRepository<TEntity> Repository { get; set; }
        Task<BaseResult<TDto>> UpdateAsync(TDto model);
    }
}
