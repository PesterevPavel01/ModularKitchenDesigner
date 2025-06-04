using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces
{
    public interface IExchangeProcessor<TDto>
        where TDto : class
    {
        IExchangeProcessor<TDto> SetProcessorFactory(IProcessorFactory processorFactory);
        Task<CollectionResult<NomanclatureDto>> ProcessAsync(List<NomanclatureDto> models, Func<NomanclatureDto, bool> isUniqueKeyEqual);
    }
}
