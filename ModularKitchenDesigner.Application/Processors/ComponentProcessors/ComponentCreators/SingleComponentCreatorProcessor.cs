using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.ComponentProcessors.ComponentCreators
{
    public sealed class SingleComponentCreatorProcessor : ICreatorProcessor<ComponentDto, BaseResult<ComponentDto>>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;
        public ICreatorProcessor<ComponentDto, BaseResult<ComponentDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<ComponentDto, BaseResult<ComponentDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<ComponentDto>> ProcessAsync(ComponentDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var componentResult = await _repositoryFactory.GetRepository<Component>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: componentResult,
                    preffix: "",
                     suffix: suffix);

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => x.Title == model.ComponentType)).FirstOrDefault(),
                    preffix: "",
                     suffix: suffix);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => x.Title == model.PriceSegment)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => x.Title == model.Material)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => x.Title == model.Model)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);


            Component componentCreatorResult = await _repositoryFactory
                .GetRepository<Component>()
                .CreateAsync(
                    new Component()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        Price = model.Price,
                        ComponentTypeId = componentTypeResult.Id,
                        PriceSegmentId = priceSegmentResult.Id,
                        MaterialId = materialResult.Id,
                        ModelId = modelResult.Id
                    });

            var newComponent = (await _repositoryFactory
                .GetRepository<Component>()
                .GetAllAsync(
                    include: Component.IncludeRequaredField(),
                    predicate: x => x.Code == model.Code
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newComponent)
            };

        }

    }
}
