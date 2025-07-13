using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Services
{
    public class ExchangeComponentPricesService : IExchangeService<CompopnentPriceDto>
    {

        private readonly IProcessorFactory _processorFactory;

        public ExchangeComponentPricesService(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }
        public async Task<CollectionResult<CompopnentPriceDto>> ExchangeAsync(List<CompopnentPriceDto> models)
        {
            var components = await _processorFactory
                    .GetLoaderProcessor<CommonLoaderWithoutValidationProcessor<Component, ComponentDto>, Component, ComponentDto>()
                    .ProcessAsync(
                        predicate: entity => models.Select(m => m.Code).Contains(entity.Code));

            if (components.Count == 0)
                throw new ArgumentException("Components not found");

            var changedComponents = components.Data.Where(entity => models.Any(m => m.Code == entity.Code && m.Price != entity.Price)).ToList();

            if (changedComponents.Any())
            {
                changedComponents.Select(component => component.UpdatePrice(models.First(model => model.Code == component.Code).Price)).ToList();

                await _processorFactory
                    .GetCreatorProcessor<CommonMultipleUpdaterProcessor<Component, ComponentDto, ComponentConverter>, Component, ComponentDto>()
                        .ProcessAsync([.. changedComponents]);
            }

            return new()
            {
                Data = models
            };
        }
    }
}
