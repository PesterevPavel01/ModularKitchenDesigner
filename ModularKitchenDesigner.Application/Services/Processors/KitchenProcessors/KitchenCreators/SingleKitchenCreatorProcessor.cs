using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.KitchenProcessors.KitchenCreators
{
    public sealed class SingleKitchenCreatorProcessor : ICreatorProcessor<KitchenDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ICreatorProcessor<KitchenDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<KitchenDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<KitchenDto>> ProcessAsync(KitchenDto model)
        {
            var kitchenResult = await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenResult,
                    preffix: "",
                    $"Object: SingleKitchenCreator.CreateAsync(KitchenDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Title == model.KitchenType)).FirstOrDefault(),
                    preffix: "",
                   $"Object: SingleKitchenCreator.CreateAsync(KitchenDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            Kitchen kitchenCreatorResult = await _repositoryFactory
                .GetRepository<Kitchen>()
                .CreateAsync(
                    new Kitchen()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        KitchenTypeId = kitchenTypeResult.Id,
                    });

            var newKitchen = (await _repositoryFactory
                .GetRepository<Kitchen>()
                .GetAllAsync(
                    include: query => query.Include(x => x.KitchenType),
                    predicate: x => x.Code == model.Code
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newKitchen)
            };
        }
    }
}