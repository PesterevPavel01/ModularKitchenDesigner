using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class MaterialSelectionItemConverter : IDtoToEntityConverter<MaterialSelectionItem, MaterialSelectionItemDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<MaterialSelectionItem, MaterialSelectionItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<MaterialSelectionItem, MaterialSelectionItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<MaterialSelectionItem>> Convert(List<MaterialSelectionItemDto> models, List<MaterialSelectionItem> entities, Func<MaterialSelectionItemDto, Func<MaterialSelectionItem, bool>> findEntityByDto, string[] validatorSuffix)
        {
            var materialResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                model: await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => models.Select(model => model.Material).Contains(x.Title)),
                preffix: "",
                suffix: validatorSuffix);

            var kitchenTypeResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                    model: await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(x => models.Select(model => model.KitchenType).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            var componentTypeResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                    model: await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(x => models.Select(model => model.ComponentType).Contains(x.Title)),
                    preffix: "",
                   suffix: validatorSuffix);

            foreach (MaterialSelectionItemDto model in models)
            {
                MaterialSelectionItem? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.MaterialId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: materialResult.Find(material => material.Title == model.Material),
                    suffix: validatorSuffix)?.Id ?? entity.MaterialId;

                entity.KitchenTypeId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: kitchenTypeResult.Find(type => type.Title == model.KitchenType),
                    suffix: validatorSuffix)?.Id ?? entity.KitchenTypeId;

                entity.ComponentTypeId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: componentTypeResult.Find(type => type.Title == model.ComponentType),
                    suffix: validatorSuffix)?.Id ?? entity.ComponentTypeId;
            }

            return entities; 
        }
    }
}
