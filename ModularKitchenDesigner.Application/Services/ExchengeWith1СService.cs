using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.Exchange;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Services
{
    public class ExchengeWith1СService : IExchangeService<NomanclatureDto>
    {
        private readonly IProcessorFactory _processorFactory;
        public ExchengeWith1СService(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }
        public async Task<CollectionResult<NomanclatureDto>> ExchangeAsync(List<NomanclatureDto> models)
        {
            var componentTypeExchangeProcessor = await new ExchangeProcessor<ComponentType, SimpleDto, SimpleEntityConverter<ComponentType>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x =>
                    x.Code == "00080200115"
                    || x.Code == "00080200116"
                    || x.Code == "00080200117"
                    || x.Code == "00080200112");

            var materialExchangeProcessor =await new ExchangeProcessor<Material, SimpleDto, SimpleEntityConverter<Material>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080200115") == 2);

            var modelExchangeProcessor = await new ExchangeProcessor<Model, SimpleDto, SimpleEntityConverter<Model>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x =>
                     (x.Parents is not null && x.Parents.Count == 2 && x.Parents.FindIndex(x => x.Code == "00080203832") == 1)
                    || (x.Parents is not null && x.Parents.Count == 3 && x.Parents?.FindIndex(x => x.Code == "00080203832") == 2));

            var moduleTypeExchangeProcessor = await new ExchangeProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080202189") == 0);

            var priceSegmentExchangeProcessor = await new ExchangeProcessor<PriceSegment, SimpleDto, SimpleEntityConverter<PriceSegment>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080200115") == 0);

            var componentExchangeProcessor = await new ExchangeProcessor<Component, ComponentDto, ComponentConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => 
                    x.Parents.FindIndex(x => x.Code == "00080200115") == 3
                    || x.Parents.FindIndex(x => x.Code == "00080200116") == 1
                    || x.Parents.FindIndex(x => x.Code == "00080200117") == 1
                    || x.Parents.FindIndex(x => x.Code == "00080200112") == 1);

            var kitchenTypeExchangeProcessor = await new ExchangeProcessor<KitchenType, KitchenTypeDto, KitchenTypeConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080200115") == 1 );
            
            var moduleExchangeProcessor = await new ExchangeProcessor<Module, ModuleDto, ModuleConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080202189") == 1);

            var materialSelectionItemExchangeProcessor = await new ExchangeProcessor<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents.FindIndex(x => x.Code == "00080200115") == 2);

            // чтобы обновить modelItems, которые содержатся в списке 
            // нужно сделать чтобы у обновляемых модулей все существующие ModelItem стали Enabled = false
            // не удаляет неактивные modelItems

            var modelItemsExchangeModels = models
                .Where(x => x.Parents
                    .FindIndex(x => x.Code == "00080202189") == 1 && x.Models is not null)
                    .SelectMany(x => x.Models
                        .Select(model =>
                            new NomanclatureDto()
                            {
                                Code = x.Code,
                                Models = [model],
                                Parents = x.Parents,
                                Title = model.Title != "removed" ? x.Title : "removed"
                            }))
                .ToList();

            var modelItemExchangeProcessor = await new ExchangeProcessor<ModelItem, ModelItemDto, ModelItemConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(
                    models: modelItemsExchangeModels, 
                    isUniqueKeyEqual: x => x.Parents.FindIndex(x => x.Code == "00080202189") == 1);

            return new() 
            {
                Count = models.Count,
                Data = models
            };
        }
        
    }
}
