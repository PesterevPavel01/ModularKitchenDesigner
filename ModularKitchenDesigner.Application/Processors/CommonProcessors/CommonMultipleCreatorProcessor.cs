using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public sealed class CommonMultipleCreatorProcessor<TEntity, TDto, TConverter> : ICreatorProcessor<TDto, TEntity>
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

        /// <summary>
        /// Процессор
        /// </summary>
        /// <param name="models">Модели, которые нужно добавить</param>
        /// <param name="findEntityByDto">Функция для сопоставления Dto с соответствующей ей Entity</param>
        /// <param name="predicate">Predicat для поиска существующих Entity, соответствующих Dto, через EF Core</param>
        /// <returns></returns>

        public async Task<CollectionResult<TDto>> ProcessAsync(List<TDto> models, Func<TDto, Func<TEntity, bool>> findEntityByDto, Expression<Func<TEntity, bool>>? predicate = null)
        {
            string[] suffix = [
                    $"Object: {GetType().Name}",
                    $"Argument: {JsonConvert.SerializeObject(models, Formatting.Indented)}"
                ];

            List<TEntity> currentEntityes = _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory
                        .GetRepository<TEntity>()
                        .GetAllAsync(
                            predicate: predicate
                        ),
                    preffix: "",
                    suffix: suffix);

            List<TEntity> newEntities = models.Select(model => new TEntity() {Id = model.GetId()}).ToList();

            if (findEntityByDto == null)
            {
                findEntityByDto = model => entity => entity.Id == model.GetId();
            }

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .CreateMultipleAsync(
                    await _converterFactory
                    .GetConverter<TEntity, TDto, TConverter>()
                    .Convert(
                        models : models, 
                        entities : newEntities, 
                        validatorSuffix : suffix,
                        findEntityByDto : findEntityByDto));

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
