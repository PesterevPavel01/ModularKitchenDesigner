using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface IProcessorFactory<TEntity, TDto>
        where TEntity : Identity, IAuditable
    {
        /*
        ICreatorProcessor<TEntity, TDto> GetCreatorProcessor<TProcessor>()
            where TProcessor : class, ICreatorProcessor<TEntity,TDto>, new();
*/        
        ILoaderProcessor<TEntity,TDto> GetLoaderProcessor<TProcessor>()
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new();

        ICreatorProcessor<TDto, TEntity> GetCreatorProcessor<TProcessor>()
            where TProcessor : class, ICreatorProcessor<TDto, TEntity>, new();
    }
}
