using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
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
        public async Task<List<Component>> Convert(List<ComponentDto> models, List<Component> entities)
        {
            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => models.Select(model => model.ComponentType).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => models.Select(model => model.PriceSegment).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => models.Select(model => model.Material).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => models.Select(model => model.Model).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<Component> components = [];

            foreach (ComponentDto componentModel in models)
            {
                var title = componentModel.Title;
                var code = componentModel.Code;
                var price = componentModel.Price;
                
                var componentType = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: componentTypeResult.Find(type => type.Title == componentModel.ComponentType),
                        methodArgument: models,
                        callerObject: GetType().Name);

                var priceSegment = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: priceSegmentResult.Find(segment => segment.Title == componentModel.PriceSegment),
                        methodArgument: models, callerObject: GetType().Name);

                var material = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: materialResult.Find(material => material.Title == componentModel.Material),
                        methodArgument: models, callerObject: GetType().Name);

                var model = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: modelResult.Find(currentModel => currentModel.Title == componentModel.Model),
                        methodArgument: models, callerObject: GetType().Name);

                Component? component = entities.Find(c => c.IsUniqueKeyEqual(componentModel));

                if (component is null)
                    components.Add(
                        Component.Create(
                            title: title,
                            code: code,
                            price: price,
                            componentType: componentType,
                            priceSegment: priceSegment,
                            material: material,
                            model: model));
                else
                    //флаг Enabled не передается, т.к. при обновлении сущности она автоматически становится Enabled = true
                    components.Add(
                        component.Update(
                            title: title,
                            code: code,
                            price: price,
                            componentType: componentType,
                            priceSegment: priceSegment,
                            material: material,
                            model: model));
            }
            
            return components;
        }
    }
}
