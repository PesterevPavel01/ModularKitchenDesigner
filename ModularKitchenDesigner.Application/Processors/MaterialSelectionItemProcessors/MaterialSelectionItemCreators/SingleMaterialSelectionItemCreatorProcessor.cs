using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.MaterialSelectionItemProcessors.MaterialSelectionItemCreators
{
    public sealed class SingleMaterialSelectionItemCreatorProcessor : ICreatorProcessor<MaterialSelectionItemDto, BaseResult<MaterialSelectionItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<MaterialSelectionItemDto, BaseResult<MaterialSelectionItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<MaterialSelectionItemDto, BaseResult<MaterialSelectionItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<MaterialSelectionItemDto>> ProcessAsync(MaterialSelectionItemDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(predicate: x => x.KitchenType.Title == model.KitchenType 
                        && x.Material.Title == model.Material
                        && x.ComponentType.Title == model.ComponentType),
                    preffix: "",
                    suffix: suffix);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => x.Title == model.Material)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Title == model.KitchenType)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => x.Title == model.ComponentType)).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            MaterialSelectionItem materialItemCreatorResult = await _repositoryFactory
                .GetRepository<MaterialSelectionItem>()
                .CreateAsync(
                    new MaterialSelectionItem()
                    {
                        MaterialId = materialResult.Id,
                        ComponentTypeId = componentTypeResult.Id,
                        KitchenTypeId = kitchenTypeResult.Id
                    });

            var newMaterialItem = (await _repositoryFactory
                .GetRepository<MaterialSelectionItem>()
                .GetAllAsync(
                    include: MaterialSelectionItem.IncludeRequaredField(),
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
