using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;

namespace ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenTypeLoader
{
    public class DefaultKitchenTypeLoaderProcessor : ILoaderProcessor<KitchenType, KitchenTypeDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ILoaderProcessor<KitchenType, KitchenTypeDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<KitchenType, KitchenTypeDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<KitchenTypeDto>> ProcessAsync(Expression<Func<KitchenType, bool>>? predicate = null)
        {
            List<KitchenType> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(
                        include: query => query.Include(x => x.PriceSegment),
                        predicate: predicate),
                    preffix: "",
                    suffix: "Object: DefaultKitchenTypeLoaderProcessor.ProcessAsync(Expression<Func<KitchenType, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new KitchenTypeDto(x))
            };
        }
    }
}