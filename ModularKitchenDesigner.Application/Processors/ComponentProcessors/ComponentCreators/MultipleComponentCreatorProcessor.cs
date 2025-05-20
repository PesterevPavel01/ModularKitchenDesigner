using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.ComponentProcessors.ComponentCreators
{
    public sealed class MultipleComponentCreatorProcessor : ICreatorProcessor<List<ComponentDto>, CollectionResult<ComponentDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<ComponentDto>, CollectionResult<ComponentDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<ComponentDto>, CollectionResult<ComponentDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<ComponentDto>> ProcessAsync(List<ComponentDto> models)
        {

            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(models, Formatting.Indented)}"
            ];

            var componentResult = await _repositoryFactory.GetRepository<Component>().GetAllAsync(predicate: x => models.Select(model => model.Code).Contains(x.Code));

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: componentResult,
                preffix: "",
                suffix: suffix);

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => !models.Select(model => model.ComponentType).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => !models.Select(model => model.PriceSegment).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => !models.Select(model => model.Material).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            
            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => !models.Select(model => model.Model).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            List<Component> componentCreatorResult = await _repositoryFactory
                .GetRepository<Component>()
                .CreateMultipleAsync(
                    models.Select(model => new Component()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        Price = model.Price,
                        ComponentTypeId = componentTypeResult.Id,
                        PriceSegmentId = priceSegmentResult.Id,
                        MaterialId = materialResult.Id,
                        ModelId = modelResult.Id
                    }).ToList());


            var newComponents = await _repositoryFactory
                .GetRepository<Component>()
                .GetAllAsync(
                    include: Component.IncludeRequaredField(),
                    predicate: x => models.Select(model => model.Code).Contains(x.Code)
                );

            return new()
            {
                Count = componentCreatorResult.Count,
                Data = newComponents.Select(component => component.ConvertToDto())
            };

        }
    }
}
