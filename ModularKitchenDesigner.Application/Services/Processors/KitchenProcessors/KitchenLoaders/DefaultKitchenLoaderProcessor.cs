using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenLoaders
{
    public sealed class DefaultKitchenLoaderProcessor : ILoaderProcessor<Kitchen, KitchenDto>
    { 
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ILoaderProcessor<Kitchen, KitchenDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<Kitchen, KitchenDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<KitchenDto>> ProcessAsync(Expression<Func<Kitchen, bool>>? predicate = null)
        {
            List<Kitchen> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(
                        include: query => query.Include(x => x.KitchenType),
                        predicate: predicate),
                    preffix: "",
                    suffix: "Object: DefaultKitchenTypeLoaderProcessor.ProcessAsync(Expression<Func<KitchenType, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new KitchenDto(x))
            };
        }
    }
}
