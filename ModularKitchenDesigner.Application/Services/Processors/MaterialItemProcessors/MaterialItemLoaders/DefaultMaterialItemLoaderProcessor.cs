using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.MaterialItemProcessors.MaterialItemLoader
{
    public class DefaultMaterialItemLoaderProcessor : ILoaderProcessor<MaterialItem, MaterialItemDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ILoaderProcessor<MaterialItem, MaterialItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<MaterialItem, MaterialItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<MaterialItemDto>> ProcessAsync(Expression<Func<MaterialItem, bool>> predicate = null)
        {
            List<MaterialItem> models = _validatorFactory.GetEmptyListValidator().Validate(
             models: await _repositoryFactory.GetRepository<MaterialItem>().GetAllAsync(
                 include: query => query.Include(x => x.Material).Include(x => x.ComponentType).Include(x => x.KitchenType),
                 predicate: predicate),
             preffix: "",
             suffix: "Object: DefaultMaterialItemLoaderProcessor.ProcessAsync(Expression<Func<MaterialItem, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new MaterialItemDto(x))
            };
        }

    }
}
