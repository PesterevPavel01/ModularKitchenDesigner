using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
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
        public async Task<List<Component>> Convert(List<ComponentDto> models, List<Component> entities, Func<ComponentDto, Func<Component, bool>> findEntityByDto)
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

            foreach (ComponentDto model in models)
            {
                Component? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.Price = model.Price;
                
                entity.ComponentTypeId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: componentTypeResult.Find(type => type.Title == model.ComponentType),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.ComponentTypeId;

                entity.PriceSegmentId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: priceSegmentResult.Find(segment => segment.Title == model.PriceSegment),
                    methodArgument: models, callerObject: GetType().Name)?.Id ?? entity.PriceSegmentId;

                entity.MaterialId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: materialResult.Find(material => material.Title == model.Material),
                    methodArgument: models, callerObject: GetType().Name)?.Id ?? entity.MaterialId;

                entity.ModelId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: modelResult.Find(currentModel => currentModel.Title == model.Model),
                    methodArgument: models, callerObject: GetType().Name)?.Id ?? entity.ModelId;
            }
            
            return entities;
        }
    }
}
