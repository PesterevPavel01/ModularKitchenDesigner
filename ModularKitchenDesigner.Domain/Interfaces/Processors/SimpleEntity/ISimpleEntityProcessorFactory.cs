using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Base;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors
{
    public interface ISimpleEntityProcessorFactory
    {
        ILoaderProcessor<TEntity, SimpleDto> GetLoaderProcessor<TProcessor,TEntity>()
            where TEntity : SimpleEntity, ISimpleEntity, IDtoConvertible<TEntity, SimpleDto>,new()
            where TProcessor : class, ILoaderProcessor<TEntity, SimpleDto>, new();

        ISimpleEntityRemoveProcessor GetRemoveProcessor<TEntity>()
            where TEntity : SimpleEntity, ISimpleEntity, IDtoConvertible<TEntity, SimpleDto>, new();
    }
}
