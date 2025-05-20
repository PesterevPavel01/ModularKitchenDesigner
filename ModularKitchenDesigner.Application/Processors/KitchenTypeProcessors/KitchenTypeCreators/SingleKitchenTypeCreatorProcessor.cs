using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.KitchenTypeProcessors.KitchenTypeCreators
{
    public sealed class SingleKitchenTypeCreatorProcessor : ICreatorProcessor<KitchenTypeDto, BaseResult<KitchenTypeDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<KitchenTypeDto, BaseResult<KitchenTypeDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<KitchenTypeDto, BaseResult<KitchenTypeDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<KitchenTypeDto>> ProcessAsync(KitchenTypeDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var kitchenTypeResult = await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenTypeResult,
                    preffix: "",
                    suffix: suffix);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => x.Title == model.PriceSegment)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            KitchenType kitchenTypeCreatorResult = await _repositoryFactory
                .GetRepository<KitchenType>()
                .CreateAsync(
                    new()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        PriceSegmentId = priceSegmentResult.Id,
                    });

            var newKitchenType = (await _repositoryFactory
                .GetRepository<KitchenType>()
                .GetAllAsync(
                    include: KitchenType.IncludeRequaredField(),
                    predicate: x => x.Code == model.Code
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newKitchenType)
            };
        }
    }
}
