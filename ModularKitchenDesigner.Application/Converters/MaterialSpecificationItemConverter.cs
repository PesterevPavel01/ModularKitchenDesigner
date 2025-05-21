using ModularKitchenDesigner.Application.Validators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class MaterialSpecificationItemConverter : IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<List<MaterialSpecificationItem>> Convert(List<MaterialSpecificationItemDto> models, List<MaterialSpecificationItem> entities, Func<MaterialSpecificationItemDto, Func<MaterialSpecificationItem, bool>> findEntityByDto, string[] validatorSuffix)
        {
            var modulTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => models.Select(model => model.ModuleType).Contains(x.Title)),
                            preffix: "",
                            suffix: validatorSuffix);

            var materialSelectionItemResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(predicate: x => models.Select(model => model.MaterialSelectionItemGuid).Contains(x.Id)),
                    preffix: "",
                    suffix: validatorSuffix);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => models.Select(model => model.KitchenGuid).Contains(x.Id)),
                    preffix: "",
                    suffix: validatorSuffix);

            foreach (MaterialSpecificationItemDto model in models)
            {
                MaterialSpecificationItem? entity = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: entities.FirstOrDefault(findEntityByDto(model)),
                        suffix: validatorSuffix,
                        preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                    );

                entity.ModuleTypeId = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: modulTypeResult.Find(x => x.Title == model.ModuleType),
                        suffix: validatorSuffix
                        )?.Id ?? entity.ModuleTypeId;

                entity.MaterialSelectionItemId = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: materialSelectionItemResult.Find(x => x.Id == model.MaterialSelectionItemGuid),
                        suffix: validatorSuffix
                        )?.Id ?? entity.MaterialSelectionItemId;

                entity.KitchenId = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: kitchenResult.Find(x => x.Id == model.KitchenGuid),
                        suffix: validatorSuffix
                        )?.Id ?? entity.KitchenId;
            }

            return entities;
        }
    }
}
