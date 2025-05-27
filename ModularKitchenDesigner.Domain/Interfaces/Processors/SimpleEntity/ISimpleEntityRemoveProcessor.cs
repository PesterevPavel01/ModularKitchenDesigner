using ModularKitchenDesigner.Domain.Dto;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors
{
    public interface ISimpleEntityRemoveProcessor
    {
        Task<BaseResult<SimpleDto>> RemoveAsync(string code);
    }
}