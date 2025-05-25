using System.Linq.Expressions;
using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    public sealed class CommonDefaultLoaderProcessor<TEntity, TDto> : ILoaderProcessor<TEntity, TDto>
        where TDto : class
        where TEntity : Identity, IAuditable, IDtoConvertible<TEntity, TDto>
    {

        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;


        public ILoaderProcessor<TEntity, TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<TEntity, TDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<TDto>> ProcessAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            List<TEntity> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(
                        include: TEntity.IncludeRequaredField(),
                        predicate: predicate),
                    methodArgument: predicate?.GetType().Name ?? "N/A",
                    callerObject: GetType().Name);

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => x.ConvertToDto())
            };
        }

    }
}
