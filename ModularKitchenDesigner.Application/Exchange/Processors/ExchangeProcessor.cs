﻿using System.Data;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Processors
{
    public class ExchangeProcessor<TEntity, TDto, TConverter> : IExchangeProcessor<TDto>
        where TDto : BaseDto, IExcangeDtoConvertable<TDto, NomanclatureDto>, IUniqueKeyQueryable<TDto>, new()
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


        public async Task<CollectionResult<NomanclatureDto>> ProcessAsync(List<NomanclatureDto> models, Func<NomanclatureDto, bool> isUniqueKeyEqual)
        {
            if(models is null)
                return new()
                {
                    Data = models
                };

            List<TDto> modelsForCreation = [];

            // Получаю все элементы, которые на момент обновления являются элементами TEntity и должны быть обновлены,
            // среди них будут в том числе те, которые перенесены в другую номенклатурную группу

            List<TDto> dtoModels = [.. models.Select(x => new TDto().Convert(x))];

            CollectionResult<TDto> modelsForUpdate = await _processorFactory
                .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                .ProcessAsync(predicate:
                    TEntity.ContainsByUniqueKeyPredicate(dtoModels));


            // Выбираем пришедшие номенклатурные позиции, только те, которые должны быть элементами TEntity ПОСЛЕ обновления

            var newAndUpdatedModelsAfterExchange = models
                .Where(isUniqueKeyEqual)
                .Select(model => new TDto().Convert(model))
                .ToList();

            if (modelsForUpdate.Count != 0)
            {
                // Выделяем те сущности, которые должны быть изменены, в том числе с Title = "removed"

                var changedModels = newAndUpdatedModelsAfterExchange
                    .Where( x => x.HasMatchingUniqueKey(modelsForUpdate.Data))
                    .ToList();

                //изменяем только те записи, у которых Title != "removed"

                if (changedModels.Any(x => x.Title != "removed" && x.Code != "removed"))
                    await _processorFactory
                        .GetCreatorProcessor<CommonMultipleUpdaterProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                        .ProcessAsync(changedModels.Where(x => x.Title != "removed" && x.Code != "removed").ToList());

                // Получаем сущности, которые деактивированы на момент обновления, у modelsBeforeUpdate точно есть код, т.к. они загружены из EF

                var disabledModels = await _processorFactory
                    .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<TEntity, TDto>, TEntity, TDto>()
                    .ProcessAsync(
                        predicate: entity => modelsForUpdate.Data.Select(x => x.Code).Contains(entity.Code)
                                   && entity.Enabled == false);

                // Выделяем те сущности, которые должны быть удалены (отключены) из активных элементов TEntity

                var changeEnableModels = modelsForUpdate.Data
                    .Where( model => !model.HasMatchingUniqueKey(newAndUpdatedModelsAfterExchange)
                            && !disabledModels.Data.Select(x => x.Code).Contains(model.Code))
                    .ToList();

                //Добавляем к ним модели, которые пришли с Title = "removed"
                changeEnableModels.AddRange(changedModels.Where(x => x.Title == "removed" || x.Code == "removed").ToList());

                /* Активация не требуется, т.к. при обновлении сущности через метод Update() она автоматически становится Enable = true*/

                // отключаем сущности

                if (changeEnableModels.Any())
                    await _processorFactory
                        .GetCreatorProcessor<CommonMultipleDisablerProcessor<TEntity, TDto>, TEntity, TDto>()
                        .ProcessAsync(changeEnableModels);

                // Выделяем те сущности, которые должны быть созданы
                modelsForCreation = newAndUpdatedModelsAfterExchange
                    .Where( x => !x.HasMatchingUniqueKey(modelsForUpdate.Data))
                    .ToList();

                if (modelsForCreation.Any())
                    await _processorFactory
                        .GetCreatorProcessor<CommonMultipleCreatorProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                        .ProcessAsync(modelsForCreation);
            }
            else
                if(newAndUpdatedModelsAfterExchange.Any())
                    await _processorFactory
                        .GetCreatorProcessor<CommonMultipleCreatorProcessor<TEntity, TDto, TConverter>, TEntity, TDto>()
                        .ProcessAsync(newAndUpdatedModelsAfterExchange); 
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
