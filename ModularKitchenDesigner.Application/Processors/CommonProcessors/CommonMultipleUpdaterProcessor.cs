using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public class CommonMultipleUpdaterProcessor<TEntity,TDto,TConverter> : ICreatorProcessor<TDto, TEntity>
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
 
            List<TEntity> currentEntities = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory
                        .GetRepository<TEntity>()
                        .GetAllAsync(
                            predicate: TEntity.ContainsByUniqueKeyPredicate(models),
                            trackingType: TrackingType.Tracking,
                            include: TEntity.IncludeRequaredField()),
                    methodArgument: models,
                    callerObject: GetType().Name);

            if (models.Count != currentEntities.Count)
            { 
                foreach (TDto model in models) 
                {
                    _validatorFactory
                       .GetObjectNullValidator()
                       .Validate(
                           model: currentEntities.Find(entity => entity.IsUniqueKeyEqual(model)),
                           methodArgument: model,
                           callerObject: GetType().Name);
                }
            }

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .UpdateMultipleAsync(
                    await _converterFactory
                    .GetConverter<TEntity, TDto, TConverter>()
                    .Convert(
                        models: models,
                        entities: currentEntities));
            
            var newComponents = await _repositoryFactory
                .GetRepository<TEntity>()
                .GetAllAsync(
                    include: TEntity.IncludeRequaredField(),
                    trackingType: TrackingType.Tracking,
                    predicate: TEntity.ContainsByUniqueKeyPredicate(models)
                );
            
            return new()
            {
                Count = newComponents.Count,
                Data = newComponents.Select(component => component.ConvertToDto())
            };
        }
    }
}
