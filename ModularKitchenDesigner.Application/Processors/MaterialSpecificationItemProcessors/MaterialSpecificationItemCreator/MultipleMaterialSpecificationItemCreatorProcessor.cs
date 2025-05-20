using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.MaterialSpecificationItemProcessors.MaterialSpecificationItemCreator
{
    public class MultipleMaterialSpecificationItemCreatorProcessor : ICreatorProcessor<List<MaterialSpecificationItemDto>, CollectionResult<MaterialSpecificationItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<MaterialSpecificationItemDto>, CollectionResult<MaterialSpecificationItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<MaterialSpecificationItemDto>, CollectionResult<MaterialSpecificationItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<MaterialSpecificationItemDto>> ProcessAsync(List<MaterialSpecificationItemDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<MaterialSpecificationItem>().GetAllAsync(
                        predicate:
                            x => data.Select(model => model.ModuleType).Contains(x.ModuleType.Title)
                            && data.Select(model => model.MaterialSelectionItemGuid).Contains(x.MaterialSelectionItem.Id)
                            && data.Select(model => model.KitchenGuid).Contains(x.Kitchen.Id)),
                    preffix: "",
                    suffix: suffix);

            var modulTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => data.Select(model => model.ModuleType).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var materialSpecificationItemResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<MaterialSpecificationItem>().GetAllAsync(predicate: x => data.Select(model => model.MaterialSelectionItemGuid).Contains(x.Id))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => data.Select(model => model.KitchenGuid).Contains(x.Id))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            List<MaterialSpecificationItem> materialSpecificationItem = await _repositoryFactory
                .GetRepository<MaterialSpecificationItem>()
                .CreateMultipleAsync(
                    data.Select(model => new
                    MaterialSpecificationItem()
                    {
                        ModuleTypeId = modulTypeResult.Id,
                        MaterialSelectionItemId = materialSpecificationItemResult.Id,
                        KitchenId = kitchenResult.Id
                    }).ToList());

            var newMaterialSpecificationItems = await _repositoryFactory
                .GetRepository<MaterialSpecificationItem>()
                .GetAllAsync(
                    include: MaterialSpecificationItem.IncludeRequaredField(),
                    predicate:
                        x => data.Select(model => model.ModuleType).Contains(x.ModuleType.Title)
                        && data.Select(model => model.MaterialSelectionItemGuid).Contains(x.MaterialSelectionItem.Id)
                        && data.Select(model => model.KitchenGuid).Contains(x.Kitchen.Id));

            return new()
            {
                Count = newMaterialSpecificationItems.Count,
                Data = newMaterialSpecificationItems.Select(materialSpecificationItems => materialSpecificationItems.ConvertToDto())
            };
        }
    }
}
