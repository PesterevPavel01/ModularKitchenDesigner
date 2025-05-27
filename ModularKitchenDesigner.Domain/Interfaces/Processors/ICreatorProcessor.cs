using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ICreatorProcessor<TDto, TEntity>
        where TEntity : IDtoConvertible<TEntity, TDto>
    {
        ICreatorProcessor<TDto, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory);
        ICreatorProcessor<TDto, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory);
        ICreatorProcessor<TDto, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<CollectionResult<TDto>> ProcessAsync(List<TDto> data);
    }
}
