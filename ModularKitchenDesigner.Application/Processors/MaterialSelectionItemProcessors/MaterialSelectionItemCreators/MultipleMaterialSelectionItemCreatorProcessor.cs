using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.MaterialSelectionItemProcessors.MaterialSelectionItemCreators
{
    public class MultipleMaterialSelectionItemCreatorProcessor : ICreatorProcessor<List<MaterialSelectionItemDto>, CollectionResult<MaterialSelectionItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<MaterialSelectionItemDto>, CollectionResult<MaterialSelectionItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public ICreatorProcessor<List<MaterialSelectionItemDto>, CollectionResult<MaterialSelectionItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }
        public async Task<CollectionResult<MaterialSelectionItemDto>> ProcessAsync(List<MaterialSelectionItemDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(
                        predicate: 
                             x => data.Select(model => model.KitchenType).Contains(x.KitchenType.Title)
                             && data.Select(model => model.Material).Contains(x.Material.Title)
                             && data.Select(model => model.ComponentType).Contains(x.ComponentType.Title)),
                    preffix: "",
                    suffix: suffix);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => data.Select(model => model.Material).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(x => data.Select(model => model.KitchenType).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(x => data.Select(model => model.ComponentType).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            List<MaterialSelectionItem> materialItemCreatorResult = await _repositoryFactory
                .GetRepository<MaterialSelectionItem>()
                .CreateMultipleAsync(
                data.Select(model => new MaterialSelectionItem()
                    {
                        MaterialId = materialResult.Id,
                        ComponentTypeId = componentTypeResult.Id,
                        KitchenTypeId = kitchenTypeResult.Id
                    }).ToList());

            var newMaterialItems = await _repositoryFactory
                .GetRepository<MaterialSelectionItem>()
                .GetAllAsync(
                    include: MaterialSelectionItem.IncludeRequaredField(),
                    predicate:
                        x => data.Select(model => model.KitchenType).Contains(x.KitchenType.Title)
                        && data.Select(model => model.Material).Contains(x.Material.Title)
                        && data.Select(model => model.ComponentType).Contains(x.ComponentType.Title));

            return new()
            {
                Count = newMaterialItems.Count,
                Data = newMaterialItems.Select(materialSelectionItem => materialSelectionItem.ConvertToDto())
            };
        }
    }
}
