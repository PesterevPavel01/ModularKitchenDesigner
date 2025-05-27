using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface IProcessorFactory
    {
        ILoaderProcessor<TEntity, TDto> GetLoaderProcessor<TProcessor, TEntity, TDto>()
            where TEntity : class, IDtoConvertible<TEntity, TDto>
            where TDto : class
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new();


        ICreatorProcessor<TDto, TEntity> GetCreatorProcessor<TProcessor, TEntity, TDto>()
            where TEntity : class, IDtoConvertible<TEntity, TDto>
            where TDto : class
            where TProcessor : class, ICreatorProcessor<TDto, TEntity>, new();
    }
}
