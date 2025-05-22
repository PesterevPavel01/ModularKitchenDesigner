using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public class CommonMultipleUpdaterProcessor<TEntity,TDto,TConverter> : ICreatorProcessor<TDto, TEntity>
        where TEntity : Identity, IConvertibleToDto<TEntity, TDto>, new()
        where TConverter : IDtoToEntityConverter<TEntity, TDto>, new()
        where TDto : IPrivateIdentity
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

        public async Task<CollectionResult<TDto>> ProcessAsync(List<TDto> models, Func<TDto, Func<TEntity, bool>> findEntityByDto, Expression<Func<TEntity, bool>>? predicate = null)
        {
 
            List<TEntity> currentEntities = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory
                        .GetRepository<TEntity>()
                        .GetAllAsync(
                            predicate:predicate,
                            trackingType: TrackingType.Tracking,
                            include: TEntity.IncludeRequaredField()
                        ),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .UpdateMultipleAsync(
                    await _converterFactory
                    .GetConverter<TEntity, TDto, TConverter>()
                    .Convert(
                        models: models,
                        entities: currentEntities,
                        findEntityByDto: findEntityByDto));
            
            var newComponents = await _repositoryFactory
                .GetRepository<TEntity>()
                .GetAllAsync(
                    include: TEntity.IncludeRequaredField(),
                    predicate: predicate
                );
            
            return new()
            {
                Count = newComponents.Count,
                Data = newComponents.Select(component => component.ConvertToDto())
            };
        }
    }
}
