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
    public class ExchangeProcessor<TEntity, TDto, TConverter> : IExchangeProcessor<NomanclatureDto>
        where TDto : BaseDto, IExcangeDtoConvertable<TDto, NomanclatureDto>, new()
        where TEntity : BaseEntity, IDtoConvertible<TEntity, TDto>, new()
        where TConverter: IDtoToEntityConverter<TEntity, TDto>, new()
    {
        private IProcessorFactory _processorFactory = null!;

        public IExchangeProcessor<NomanclatureDto> SetProcessorFactory(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
            return this;
        }

        public async Task<CollectionResult<NomanclatureDto>> ProcessAsync(List<NomanclatureDto> models, Func<NomanclatureDto, bool> isUniqueKeyEqual)
        {
            // Получаю все элементы, которые на момент обновления являются элементами TEntity и должны быть обновлены
            var modelsBeforeUpdate = await _processorFactory
                .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                .ProcessAsync(predicate: entity => models.Select(x => x.Code).Contains(entity.Code));

            // Выбираем пришедшие номенклатурные позиции, которые должны быть элементами TEntity после обновления
            var changedAndCreatedModels = models
                .Where(isUniqueKeyEqual)
                .Select(model => new TDto().Convert(model))
                .ToList();

            // Выделяем те сущности, которые должны быть изменены
            var changedModels = changedAndCreatedModels
                .Where(
                    x => modelsBeforeUpdate.Data.Select(model => model.Code).Contains(x.Code))
                .ToList();

            if (changedModels.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleUpdaterProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                    .ProcessAsync(changedModels);

            // Получаем сущности, которые деактивированы на момент обновления
            var disabledModels = await _processorFactory
                .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                .ProcessAsync(
                    predicate: entity => models.Select(x => x.Code).Contains(entity.Code)
                               && entity.Enabled == false);

            // Выделяем те сущности, которые должны быть удалены (отключены) из активных элементов TEntity
            var changeEnableModels = modelsBeforeUpdate.Data
                .Where(
                    model => !changedAndCreatedModels.Select(x => x.Code).Contains(model.Code)
                    && !disabledModels.Data.Select(x => x.Code).Contains(model.Code))
                .ToList();

            changeEnableModels.AddRange(
                changedModels
                    .Where(x => disabledModels.Data.Select(model => model.Code).Contains(x.Code))
                    .ToList());

            // Деактивируем сущности
            if (changeEnableModels.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleChangeEnableProcessor<TEntity, TDto>, TEntity, TDto>()
                    .ProcessAsync(changeEnableModels);

            // Выделяем те сущности, которые должны быть созданы
            var modelsToCreate = changedAndCreatedModels
                .Where(x => !modelsBeforeUpdate.Data.Select(model => model.Code).Contains(x.Code))
                .ToList();

            if (modelsToCreate.Any())
                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleCreatorProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                    .ProcessAsync(modelsToCreate);
            /*
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
