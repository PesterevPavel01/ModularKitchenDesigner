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
    public class CommonMultipleRemoveProcessor<TEntity, TDto> : ILoaderProcessor<TEntity, TDto>
        where TDto : class
        where TEntity : Identity, IAuditable, IConvertibleToDto<TEntity, TDto>
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
                predicate: predicate,
                trackingType: TrackingType.Tracking,
                include: TEntity.IncludeRequaredField()),
                methodArgument: predicate.GetType().Name,
                callerObject: GetType().Name);

            var result = await _repositoryFactory.GetRepository<TEntity>().RemoveMultipleAsync(models);

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => x.ConvertToDto())
            };
        }
    }
}
