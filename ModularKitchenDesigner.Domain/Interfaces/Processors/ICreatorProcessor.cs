using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ICreatorProcessor<TDto, TEntity>
        where TEntity : Identity
    {
        ICreatorProcessor<TDto, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory);
        ICreatorProcessor<TDto, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory);
        ICreatorProcessor<TDto, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<CollectionResult<TDto>> ProcessAsync(List<TDto> data, Func<TDto, Func<TEntity, bool>> findEntityByDto, Expression<Func<TEntity, bool>>? predicate = null);
    }
}
