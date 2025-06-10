using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CommonProcessors
{
    internal class CommonLoaderWithoutValidationProcessor<TEntity, TDto> : ILoaderProcessor<TEntity, TDto>
        where TDto : class
        where TEntity : class, IDtoConvertible<TEntity, TDto>
    {

        private IRepositoryFactory _repositoryFactory = null!;


        public ILoaderProcessor<TEntity, TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<TEntity, TDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            return this;
        }
        public async Task<CollectionResult<TDto>> ProcessAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            List<TEntity> models = await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(
                include: TEntity.IncludeRequaredField(),
                predicate: predicate,
                trackingType: TrackingType.Tracking);

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => x.ConvertToDto())
            };
        }

    }
}