using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface IProcessorFactory<TEntity, TDto>
        where TEntity : Identity, IAuditable
    {
        ICreatorProcessor<TData, TResult> GetCreatorProcessor<TProcessor, TResult, TData>()
            where TProcessor : class, ICreatorProcessor<TData, TResult>, new()
            where TResult : BaseResult;
        ILoaderProcessor<TEntity,TDto> GetLoaderProcessor<TProcessor>()
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new();

        public IUpdaterProcessor<List<TDto>, TResult, TEntity> GetUpdaterProcessor<TProcessor, TResult>()
            where TProcessor : class, IUpdaterProcessor<List<TDto>, TResult, TEntity>, new()
            where TResult : BaseResult;
    }
}
