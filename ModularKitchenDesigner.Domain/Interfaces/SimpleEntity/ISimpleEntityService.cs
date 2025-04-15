namespace ModularKitchenDesigner.Domain.Interfaces.SimpleEntity
{
    public interface ISimpleEntityService
    {
        ISimpleEntityLoader<TEntity, TDto> GetLoader<TEntity, TDto>()
            where TEntity : class, ISimpleEntity, new()
            where TDto : class, ISimpleEntity, new();

        ISimpleEntityCreator<TEntity, TDto> GetCreator<TEntity, TDto>() 
            where TEntity : class, ISimpleEntity, new() 
            where TDto : class, ISimpleEntity, new();

        ISimpleEntityUpdater<TEntity, TDto> GetUpdater<TEntity, TDto>()
            where TEntity : class, ISimpleEntity, new()
            where TDto : class, ISimpleEntity, new();
    }
}
