using Interceptors;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ILoaderProcessor<TEntity,TDto>
        where TEntity : Identity, IAuditable
        where TDto : class
    {
        ILoaderProcessor<TEntity, TDto> SetValidatorFactory(IValidatorFactory validatorFactory);
        ILoaderProcessor<TEntity, TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<CollectionResult<TDto>> ProcessAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
