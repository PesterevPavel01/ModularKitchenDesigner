using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class ComponentConverter : IDtoToEntityConverter<Component, ComponentDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<Component, ComponentDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<Component, ComponentDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<List<Component>> Convert(List<ComponentDto> models, List<Component> entities, Func<ComponentDto, Func<Component, bool>> findEntityByDto, string[] validatorSuffix)
        {
            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => models.Select(model => model.ComponentType).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => models.Select(model => model.PriceSegment).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => models.Select(model => model.Material).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => models.Select(model => model.Model).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            foreach (ComponentDto model in models)
            {
                Component? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.Price = model.Price;
                
                entity.ComponentTypeId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: componentTypeResult.Find(type => type.Title == model.ComponentType),
                    suffix: validatorSuffix)?.Id ?? entity.ComponentTypeId;

                entity.PriceSegmentId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: priceSegmentResult.Find(segment => segment.Title == model.PriceSegment),
                    suffix: validatorSuffix)?.Id ?? entity.PriceSegmentId;

                entity.MaterialId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: materialResult.Find(material => material.Title == model.Material),
                    suffix: validatorSuffix)?.Id ?? entity.MaterialId;

                entity.ModelId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: modelResult.Find(currentModel => currentModel.Title == model.Model),
                    suffix: validatorSuffix)?.Id ?? entity.ModelId;
            }
            
            return entities;
        }
    }
}
