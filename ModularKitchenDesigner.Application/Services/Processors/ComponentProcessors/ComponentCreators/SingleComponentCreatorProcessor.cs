using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Services.Processors.ConponentProcessors.ConponentCreators
{
    public sealed class SingleComponentCreatorProcessor : ICreatorProcessor<ComponentDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;
        public ICreatorProcessor<ComponentDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<ComponentDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<ComponentDto>> ProcessAsync(ComponentDto model)
        {
            var componentResult = await _repositoryFactory.GetRepository<Component>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: componentResult,
                    preffix: "",
                    $"Object: SingleComponentCreatorProcessor.CreateAsync(ComponentDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => x.Title == model.ComponentType)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleComponentCreatorProcessor.CreateAsync(ComponentDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => x.Title == model.PriceSegment)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleComponentCreatorProcessor.CreateAsync(ComponentDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var materialResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Material>().GetAllAsync(predicate: x => x.Title == model.Material)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleComponentCreatorProcessor.CreateAsync(ComponentDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => x.Title == model.Model)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleComponentCreatorProcessor.CreateAsync(ComponentDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");


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
                    include: query => query.Include(x => x.ComponentType).Include(x => x.PriceSegment).Include(x => x.Material).Include(x => x.Model),
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
