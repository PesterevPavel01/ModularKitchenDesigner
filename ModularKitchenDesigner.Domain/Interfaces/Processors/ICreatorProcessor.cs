using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ICreatorProcessor<TDto>
        where TDto : class
    {
        ICreatorProcessor<TDto> SetValidatorFactory(IValidatorFactory validatorFactory);
        ICreatorProcessor<TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<BaseResult<TDto>> ProcessAsync(TDto model);
    }
}
