using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.KitchenProcessors.KitchenCreators
{
    public sealed class SingleKitchenCreatorProcessor : ICreatorProcessor<KitchenDto, BaseResult<KitchenDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<KitchenDto, BaseResult<KitchenDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<KitchenDto, BaseResult<KitchenDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<KitchenDto>> ProcessAsync(KitchenDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var kitchenResult = await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => x.Id== model.Guid);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenResult,
                    preffix: "",
                    suffix: suffix);

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Title == model.KitchenType)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            Kitchen kitchenCreatorResult = await _repositoryFactory
                .GetRepository<Kitchen>()
                .CreateAsync(
                    new Kitchen()
                    {
                        UserLogin = model.UserLogin,
                        UserId = model.UserId,
                        KitchenTypeId = kitchenTypeResult.Id,
                    });

            var newKitchen = (await _repositoryFactory
                .GetRepository<Kitchen>()
                .GetAllAsync(
                    include: Kitchen.IncludeRequaredField(),
                    predicate: x => x.UserId == model.UserId
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newKitchen)
            };
        }
    }
}