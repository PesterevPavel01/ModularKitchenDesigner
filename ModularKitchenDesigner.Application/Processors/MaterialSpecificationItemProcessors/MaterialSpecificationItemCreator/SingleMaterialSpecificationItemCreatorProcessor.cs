using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ModularKitchenDesigner.Application.Processors.MaterialSpecificationItemProcessors.MaterialSpecificationItemCreator
{
    public class SingleMaterialSpecificationItemCreatorProcessor : ICreatorProcessor<MaterialSpecificationItemDto, BaseResult<MaterialSpecificationItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;


        public ICreatorProcessor<MaterialSpecificationItemDto, BaseResult<MaterialSpecificationItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<MaterialSpecificationItemDto, BaseResult<MaterialSpecificationItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<BaseResult<MaterialSpecificationItemDto>> ProcessAsync(MaterialSpecificationItemDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<MaterialSpecificationItem>().GetAllAsync(
                        predicate: x => x.ModuleType.Title == model.ModuleType
                        && x.MaterialSelectionItem.Id == model.MaterialSelectionItemGuid
                        && x.Kitchen.Id == model.KitchenGuid),
                    preffix: "",
                    suffix:suffix);

            var modulTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => x.Title == model.ModuleType)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var materialSpecificationItemResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<MaterialSpecificationItem>().GetAllAsync(predicate: x => x.Id == model.MaterialSelectionItemGuid)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => x.Id == model.KitchenGuid)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            MaterialSpecificationItem materialSpecificationItem = await _repositoryFactory
                .GetRepository<MaterialSpecificationItem>()
                .CreateAsync(
                    new MaterialSpecificationItem()
                    {
                        ModuleTypeId = modulTypeResult.Id,
                        MaterialSelectionItemId = materialSpecificationItemResult.Id,
                        KitchenId = kitchenResult.Id
                    });

            var newMaterialSpecificationItem = (await _repositoryFactory
                .GetRepository<MaterialSpecificationItem>()
                .GetAllAsync(
                    include: MaterialSpecificationItem.IncludeRequaredField(),
                    predicate: 
                        x => x.ModuleType.Title == model.ModuleType
                        && x.MaterialSelectionItem.Id == model.MaterialSelectionItemGuid
                        && x.Kitchen.Id == model.KitchenGuid))
                .FirstOrDefault();

            return new()
            {
                Data = new(newMaterialSpecificationItem)
            };
        }

    }
}
