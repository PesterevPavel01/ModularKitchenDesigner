using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Processors.Exchange
{
    public class ExchangeProcessor<TEntity, TDto, TConverter> : IExchangeProcessor<TDto>
        where TDto : BaseDto, IExcangeDtoConvertable<TDto, NomanclatureDto>, new()
        where TEntity : BaseEntity, IDtoConvertible<TEntity, TDto>
        where TConverter: IDtoToEntityConverter<TEntity, TDto>, new()
    {
        private IProcessorFactory _processorFactory = null!;

        public IExchangeProcessor<TDto> SetProcessorFactory(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
            return this;
        }
       
        /// <summary>
        /// Процессор последовательно запускает другие узкоспециализированные процессоры (создание, обновление, удаление сущности)
        /// </summary>
        /// <param name="models"></param>
        /// <param name="isUniqueKeyEqual"></param>
        /// <returns></returns>


        public async Task<CollectionResult<NomanclatureDto>> ProcessAsync(List<NomanclatureDto> models, Func<NomanclatureDto, bool> isUniqueKeyEqual, Func<TDto, bool> ? isElementInList = null)
        {
            // Получаю все элементы, которые на момент обновления являются элементами TEntity и должны быть обновлены
            CollectionResult<TDto> modelsBeforeUpdate = await _processorFactory
                    .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                    .ProcessAsync(
                        predicate : isElementInList is null ? 
                            entity => models.Select(x => x.Code).Contains(entity.Code) 
                            : TEntity.ContainsByUniqueKeyPredicate(models.Select(x => new TDto().Convert(x)).ToList()));

            // Выбираем пришедшие номенклатурные позиции, которые должны быть элементами TEntity после обновления
            var changedAndCreatedModels = models
                .Where(isUniqueKeyEqual)
                .Select(model => new TDto().Convert(model))
                .ToList();

            // Выделяем те сущности, которые должны быть изменены, в том числе с Title = "removed"
            var changedModels = changedAndCreatedModels
                .Where( isElementInList is null ?
                    x => modelsBeforeUpdate.Data.Select(model => model.Code).Contains(x.Code)
                    : isElementInList)
                .ToList();

            //изменяем только те записи, у которых Title != "removed"
            if (changedModels.Where(x => x.Title != "removed").Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleUpdaterProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                    .ProcessAsync(changedModels.Where(x => x.Title != "removed").ToList());

            // Получаем сущности, которые деактивированы на момент обновления, у modelsBeforeUpdate точно есть код, т.к. они загружены из EF
            var disabledModels = await _processorFactory
                .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                .ProcessAsync(
                    predicate: entity => modelsBeforeUpdate.Data.Select(x => x.Code).Contains(entity.Code)
                               && entity.Enabled == false);

            // Выделяем те сущности, которые должны быть удалены (отключены) из активных элементов TEntity
            var changeEnableModels = modelsBeforeUpdate.Data
                .Where(
                    isElementInList is null ?
                    model => !changedAndCreatedModels.Select(x => x.Code).Contains(model.Code)
                    && !disabledModels.Data.Select(x => x.Code).Contains(model.Code)
                    : model => !isElementInList(model)
                    && !disabledModels.Data.Select(x => x.Code).Contains(model.Code))
                .ToList();

            //Добавляем к ним модели, которые пришли с Title = "removed"
            changeEnableModels.AddRange(changedModels.Where(x => x.Title == "removed").ToList());

            /*
             * Активация не требуется, т.к. при обновлении сущности через метод Update() она автоматически становится Enable = true
             
            changeEnableModels.AddRange(
                changedModels
                    .Where(x => disabledModels.Data.Select(model => model.Code).Contains(x.Code))
                    .ToList());
            */

            // отключаем сущности

            if (changeEnableModels.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleDisablerProcessor<TEntity, TDto>, TEntity, TDto>()
                    .ProcessAsync(changeEnableModels);

            // Выделяем те сущности, которые должны быть созданы
            var modelsToCreate = changedAndCreatedModels
                .Where(
                    isElementInList is null ? 
                    x => !modelsBeforeUpdate.Data.Select(model => model.Code).Contains(x.Code)
                    : x=> !isElementInList(x))
                .ToList();

            if (modelsToCreate.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleCreatorProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                    .ProcessAsync(modelsToCreate);
            
            /*
             * вместо удаления необходимо пользоваться методом отключения сущности
         
            var priceSegmentsToRemove = enabledModels.Where(x => x.Title != "removed").ToList();

            if (priceSegmentsToRemove.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleRemoveProcessor<TEntity, TDto>, TEntity, TDto > ()
                    .ProcessAsync(priceSegmentsToRemove);
            */
            return new()
            {
                Count = models.Count,
                Data = models
            };
        }
    }
}
