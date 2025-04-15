using Interceptors;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface IProcessorFactory<TEntity, TDto>
        where TEntity : Identity, IAuditable
        where TDto : class
    {
        ICreatorProcessor<TDto> GetCreatorProcessor<TProcessor>()
            where TProcessor : class, ICreatorProcessor<TDto>, new();
        ILoaderProcessor<TEntity,TDto> GetLoaderProcessor<TProcessor>()
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new();
    }
}
