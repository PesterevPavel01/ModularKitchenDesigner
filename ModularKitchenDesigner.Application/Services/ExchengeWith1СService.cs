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
            var priceSegmentExchangeProcessor = await new ExchangeProcessor<PriceSegment, SimpleDto, SimpleEntityConverter<PriceSegment>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents[0].Code == "00080200257");

            var moduleTypeExchangeProcessor = await new ExchangeProcessor<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents[0].Code == "00080202192");

            var modelExchangeProcessor = await new ExchangeProcessor<Model, SimpleDto, SimpleEntityConverter<Model>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents[1].Code == "00080200257");

            var materialExchangeProcessor =await new ExchangeProcessor<Material, SimpleDto, SimpleEntityConverter<Material>>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents[1].Code == "00080200115");

            return new() 
            {
                Count = models.Count,
                Data = models
            };
        }
        
    }
}
