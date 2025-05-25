using System.Collections.Generic;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
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

        public async Task<List<Kitchen>> Convert(List<KitchenDto> models, List<Kitchen> entities)
        {
            var kitchenTypeResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                model: await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => models.Select(model => model.KitchenType).Contains(x.Title)),
                methodArgument: models,
                callerObject: GetType().Name);

            List <Kitchen> kitchens = [];

            foreach (KitchenDto model in models)
            { 
                var userLogin = model.UserLogin;
                var userId = model.UserId;
                var title = model.Title;
                var code = model.Code;

                var kitchenType = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: kitchenTypeResult.Find(type => type.Title == model.KitchenType),
                    methodArgument: models,
                    callerObject: GetType().Name);

                Kitchen? kitchen = entities.Find(entity => entity.isUniqueKeyEqual(model));

                if (kitchen is null)
                    kitchens.Add(
                        Kitchen.Create(
                            userLogin: userLogin,
                            userId: userId,
                            title: title,
                            kitchenType: kitchenType,
                            code: code));
                else
                    kitchens.Add(
                        kitchen.Update(
                            userLogin: userLogin,
                            userId: userId,
                            title: title,
                            kitchenType: kitchenType,
                            code: code));
            }

            return kitchens;
        }
    }
}
