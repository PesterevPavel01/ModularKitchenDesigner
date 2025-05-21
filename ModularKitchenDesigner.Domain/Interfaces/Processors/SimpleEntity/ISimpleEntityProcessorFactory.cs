using Interceptors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity
{
    public interface ISimpleEntityProcessorFactory
    {
        ILoaderProcessor<TEntity, SimpleDto> GetLoaderProcessor<TProcessor,TEntity>()
            where TEntity : Identity, ISimpleEntity, IAuditable
            where TProcessor : class, ILoaderProcessor<TEntity, SimpleDto>, new();

        ISimpleEntityRemoveProcessor GetRemoveProcessor<TEntity>()
            where TEntity : class, ISimpleEntity, new();
    }
}
