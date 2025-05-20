using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public class CommonMultipleUpdaterProcessor<TEntity,TDto,TConverter> : IUpdaterProcessor<List<TDto>, CollectionResult<TDto>, TEntity>
        where TEntity : class, IConvertibleToDto<TEntity, TDto>, new()
        where TConverter : IDtoToEntityConverter<TEntity, TDto>, new()
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        private IDtoToEntityConverterFactory _converterFactory = null!;

        public IUpdaterProcessor<List<TDto>, CollectionResult<TDto>, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IUpdaterProcessor<List<TDto>, CollectionResult<TDto>, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public IUpdaterProcessor<List<TDto>, CollectionResult<TDto>, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory)
        {
            _converterFactory = converterFactory;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="predicate">Условие для поиска по уникальному идентификатору, например по коду</param>
        /// <returns></returns>
        public async Task<CollectionResult<TDto>> ProcessAsync(List<TDto> models, Expression<Func<TEntity, bool>>? predicate = null)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(models, Formatting.Indented)}"
            ];

            List<TEntity> currentEntityes = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory
                        .GetRepository<TEntity>()
                        .GetAllAsync(
                            predicate:predicate,
                            trackingType: TrackingType.Tracking,
                            include: TEntity.IncludeRequaredField()
                        ),
                    preffix: "",
                    suffix: suffix);

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .UpdateMultipleAsync(await _converterFactory.GetConverter<TEntity, TDto, TConverter>().Convert(models, currentEntityes, suffix));
            
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
