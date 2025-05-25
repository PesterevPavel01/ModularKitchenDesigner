using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ILoaderProcessor<TEntity,TDto>
        where TEntity : Identity, IDtoConvertible<TEntity, TDto>
    {
        ILoaderProcessor<TEntity, TDto> SetValidatorFactory(IValidatorFactory validatorFactory);
        ILoaderProcessor<TEntity, TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<CollectionResult<TDto>> ProcessAsync(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
