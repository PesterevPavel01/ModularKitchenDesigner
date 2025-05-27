using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public sealed class CommonMultipleCreatorProcessor<TEntity, TDto, TConverter> : ICreatorProcessor<TDto, TEntity>
        where TEntity : class, IDtoConvertible<TEntity, TDto>
        where TConverter : IDtoToEntityConverter<TEntity, TDto>, new()
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        private IDtoToEntityConverterFactory _converterFactory = null!;

        public ICreatorProcessor<TDto, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<TDto, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public ICreatorProcessor<TDto, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory)
        {
            _converterFactory = converterFactory;
            return this;
        }

        public async Task<CollectionResult<TDto>> ProcessAsync(List<TDto> models)
        {
            List <TEntity> currentEntityes = _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory
                        .GetRepository<TEntity>()
                        .GetAllAsync( predicate: TEntity.ContainsByUniqueKeyPredicate(models)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .CreateMultipleAsync(
                    await _converterFactory
                    .GetConverter<TEntity, TDto, TConverter>()
                    .Convert(
                        models: models,
                        entities: currentEntityes));

            var newEntities = await _repositoryFactory
                .GetRepository<TEntity>()
                .GetAllAsync(
                    include: TEntity.IncludeRequaredField(),
                    predicate: TEntity.ContainsByUniqueKeyPredicate(models));

            return new()
            {
                Count = newEntities.Count,
                Data = newEntities.Select(entity => entity.ConvertToDto())
            };
        }
    }
}
