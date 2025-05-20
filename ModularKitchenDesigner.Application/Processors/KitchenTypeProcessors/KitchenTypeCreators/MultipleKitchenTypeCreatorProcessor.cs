using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.KitchenTypeProcessors.KitchenTypeCreators
{
    public class MultipleKitchenTypeCreatorProcessor : ICreatorProcessor<List<KitchenTypeDto>, CollectionResult<KitchenTypeDto>>
    {

        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<KitchenTypeDto>, CollectionResult<KitchenTypeDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<KitchenTypeDto>, CollectionResult<KitchenTypeDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<KitchenTypeDto>> ProcessAsync(List<KitchenTypeDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            var kitchenTypeResult = await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => data.Select(model => model.Code).Contains(x.Code));

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenTypeResult,
                    preffix: "",
                    suffix: suffix);

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => data.Select(model => model.PriceSegment).Contains(x.Code))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            List<KitchenType> kitchenTypeCreatorResult = await _repositoryFactory
                .GetRepository<KitchenType>()
                .CreateMultipleAsync( data.Select( model =>
                    new KitchenType()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        PriceSegmentId = priceSegmentResult.Id,
                    }).ToList());

            var newKitchenType = await _repositoryFactory
                .GetRepository<KitchenType>()
                .GetAllAsync(
                    include: KitchenType.IncludeRequaredField(),
                    predicate: x => data.Select(model => model.Code).Contains(x.Code)
                );

            return new()
            {
                Count = kitchenTypeCreatorResult.Count,
                Data = newKitchenType.Select(x => x.ConvertToDto())
            };
        }

    }
}
