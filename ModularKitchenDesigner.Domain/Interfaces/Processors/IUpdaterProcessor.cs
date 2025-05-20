using ModularKitchenDesigner.Domain.Entityes.Base;
using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Domain.Interfaces.Processors
{
    public interface IUpdaterProcessor<TData, TResult, TEntity>
        where TResult : BaseResult
    {
        IUpdaterProcessor<TData, TResult, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory);
        IUpdaterProcessor<TData, TResult, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory);
        IUpdaterProcessor<TData, TResult, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
        Task<TResult> ProcessAsync(TData data, Expression<Func<TEntity, bool>>? predicate = null);
    }
}
