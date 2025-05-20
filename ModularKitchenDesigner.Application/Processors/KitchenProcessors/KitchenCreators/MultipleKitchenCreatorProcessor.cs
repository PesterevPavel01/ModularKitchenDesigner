using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.KitchenProcessors.KitchenCreators
{
    public class MultipleKitchenCreatorProcessor : ICreatorProcessor<List<KitchenDto>, CollectionResult<KitchenDto>>
    {

        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<KitchenDto>, CollectionResult<KitchenDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<KitchenDto>, CollectionResult<KitchenDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<KitchenDto>> ProcessAsync(List<KitchenDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            var kitchenResult = await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => data.Select(model => model.Guid).Contains(x.Id));

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenResult,
                    preffix: "",
                    suffix: suffix);

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => data.Select(model => model.KitchenType).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            List<Kitchen> kitchenCreatorResult = await _repositoryFactory
                .GetRepository<Kitchen>()
                .CreateMultipleAsync(
                    data.Select( model => new Kitchen()
                    {
                        UserLogin = model.UserLogin,
                        UserId = model.UserId,
                        KitchenTypeId = kitchenTypeResult.Id,
                    }).ToList());

            var newKitchens = await _repositoryFactory
                .GetRepository<Kitchen>()
                .GetAllAsync(
                    include: Kitchen.IncludeRequaredField(),
                    predicate: x => data.Select(model => model.Guid).Contains(x.Id)
                );

            return new()
            {
                Count = kitchenCreatorResult.Count,
                Data = newKitchens.Select( kitchen => kitchen.ConvertToDto())
            };
        }
    }
}
