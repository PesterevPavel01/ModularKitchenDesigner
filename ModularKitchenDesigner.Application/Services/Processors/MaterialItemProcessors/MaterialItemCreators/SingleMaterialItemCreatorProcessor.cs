using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Services.Processors.MaterialItemProcessors.MaterialItemCreator
{
    public sealed class SingleMaterialItemCreatorProcessor : ICreatorProcessor<MaterialItemDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ICreatorProcessor<MaterialItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<MaterialItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<MaterialItemDto>> ProcessAsync(MaterialItemDto model)
        {
            var materialItemResult = await _repositoryFactory
                .GetRepository<MaterialItem>()
                .GetAllAsync(predicate:
                    x => x.KitchenType.Title == model.KitchenType
                    && x.Material.Title == model.Material
                    && x.ComponentType.Title == model.ComponentType);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: materialItemResult,
                    preffix: "",
                    $"Object: SingleMaterialItemCreatorProcessor.ProcessAsync(MaterialItemDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => x.Title == model.Material)).FirstOrDefault(),
                    preffix: "",
                   $"Object: SingleMaterialItemCreatorProcessor.ProcessAsync(MaterialItemDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Title == model.KitchenType)).FirstOrDefault(),
                    preffix: "",
                   $"Object: SingleMaterialItemCreatorProcessor.ProcessAsync(MaterialItemDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => x.Title == model.ComponentType)).FirstOrDefault(),
                    preffix: "",
                   $"Object: SingleMaterialItemCreatorProcessor.ProcessAsync(MaterialItemDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            MaterialItem materialItemCreatorResult = await _repositoryFactory
                .GetRepository<MaterialItem>()
                .CreateAsync(
                    new MaterialItem()
                    {
                        MaterialId = materialResult.Id,
                        ComponentTypeId = componentTypeResult.Id,
                        KitchenTypeId = kitchenTypeResult.Id,
                    });

            var newMaterialItem = (await _repositoryFactory
                .GetRepository<MaterialItem>()
                .GetAllAsync(
                    include: query => query.Include(x => x.Material).Include(x => x.ComponentType).Include(x => x.KitchenType),
                    predicate:
                        x => x.KitchenType.Title == model.KitchenType
                        && x.Material.Title == model.Material
                        && x.ComponentType.Title == model.ComponentType))
                .FirstOrDefault();

            return new()
            {
                Data = new(newMaterialItem)
            };
        }
    }
}
