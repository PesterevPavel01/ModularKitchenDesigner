using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class KitchenConverter : IDtoToEntityConverter<Kitchen, KitchenDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<Kitchen, KitchenDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<Kitchen, KitchenDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<Kitchen>> Convert(List<KitchenDto> models, List<Kitchen> entities, Func<KitchenDto, Func<Kitchen, bool>> findEntityByDto)
        {
            var kitchenTypeResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                model: await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => models.Select(model => model.KitchenType).Contains(x.Title)),
                methodArgument: models,
                callerObject: GetType().Name);

            foreach (KitchenDto model in models)
            { 
                Kitchen? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.UserLogin = model.UserLogin;
                entity.UserId = model.UserId;

                entity.KitchenTypeId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: kitchenTypeResult.Find(type => type.Title == model.KitchenType),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.KitchenTypeId;
            }

            return entities;
        }
    }
}
