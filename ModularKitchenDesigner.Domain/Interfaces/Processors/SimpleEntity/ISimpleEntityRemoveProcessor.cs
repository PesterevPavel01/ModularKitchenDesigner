using ModularKitchenDesigner.Domain.Dto;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity
{
    public interface ISimpleEntityRemoveProcessor
    {
        Task<BaseResult<SimpleDto>> RemoveAsync(string code);
    }
}