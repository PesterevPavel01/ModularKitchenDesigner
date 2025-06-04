using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Exchange
{
    public interface IExchangeService<TDto>
        where TDto : class, new()
    {
        Task<CollectionResult<TDto>> ExchangeAsync(List<TDto> models);
    }
}
