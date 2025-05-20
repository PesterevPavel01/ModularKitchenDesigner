using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface ICreatorProcessor<TData, TResult>
        where TResult : BaseResult
    {
        ICreatorProcessor<TData, TResult> SetValidatorFactory(IValidatorFactory validatorFactory);
        ICreatorProcessor<TData, TResult> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<TResult> ProcessAsync(TData data);
    }
}
