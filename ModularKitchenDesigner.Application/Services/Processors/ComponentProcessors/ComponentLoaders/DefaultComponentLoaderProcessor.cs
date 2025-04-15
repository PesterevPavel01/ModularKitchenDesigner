using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;

namespace ModularKitchenDesigner.Application.Services.Processors.ComponentProcessors.ComponentLoaders
{
    public sealed class DefaultComponentLoaderProcessor : ILoaderProcessor<Component, ComponentDto>
    {

        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ILoaderProcessor<Component, ComponentDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<Component, ComponentDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<ComponentDto>> ProcessAsync(Expression<Func<Component, bool>>? predicate = null)
        {
            List<Component> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                models: await _repositoryFactory.GetRepository<Component>().GetAllAsync(
                    include: query => query.Include(x => x.ComponentType).Include(x => x.PriceSegment).Include(x => x.Material).Include(x => x.Model),
                    predicate: predicate),
                preffix: "",
                suffix: "Object: DefaultComponentLoaderProcessor.ProcessAsync(Expression<Func<Component, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new ComponentDto(x))
            };
        }
    }
}
