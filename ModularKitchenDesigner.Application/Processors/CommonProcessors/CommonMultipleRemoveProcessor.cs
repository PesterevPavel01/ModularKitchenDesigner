using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public class CommonMultipleRemoveProcessor<TEntity, TDto> : ICreatorProcessor<TDto, TEntity>
        where TDto : class
        where TEntity : class, IDtoConvertible<TEntity, TDto>
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

            List<TEntity> entityes = _validatorFactory
            .GetEmptyListValidator()
            .Validate(
                models: await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(
                predicate: TEntity.ContainsByUniqueKeyPredicate(models),
                trackingType: TrackingType.Tracking,
                include: TEntity.IncludeRequaredField()),
                methodArgument: models?.GetType().Name ?? "N/A",
                callerObject: GetType().Name);

            var result = await _repositoryFactory.GetRepository<TEntity>().RemoveMultipleAsync(entityes);

            return new()
            {
                Count = entityes.Count,
                Data = entityes.Select(entity => entity.ConvertToDto())
            };
        }

    }
}
