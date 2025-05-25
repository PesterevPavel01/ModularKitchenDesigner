using System.Collections.Generic;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class MaterialSpecificationItemConverter : IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<MaterialSpecificationItem, MaterialSpecificationItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<List<MaterialSpecificationItem>> Convert(List<MaterialSpecificationItemDto> models, List<MaterialSpecificationItem> entities)
        {
            var modulTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => models.Select(model => model.ModuleType).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var materialSelectionItemResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(predicate: x => models.Select(model => model.MaterialSelectionItemCode).Contains(x.Code)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory
                        .GetRepository<Kitchen>()
                        .GetAllAsync(predicate: x => models.Select(model => model.KitchenCode).Contains(x.Code)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<MaterialSpecificationItem> materialSpecificationItems = [];

            foreach (MaterialSpecificationItemDto model in models)
            {
                var moduleType = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: modulTypeResult.Find(x => x.Title == model.ModuleType),
                        methodArgument: models,
                        callerObject: GetType().Name);

                var materialSelectionItem = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: materialSelectionItemResult.Find(x => x.Code == model.MaterialSelectionItemCode),
                        methodArgument: models,
                        callerObject: GetType().Name);

                var kitchen = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: kitchenResult.Find(x => x.Code == model.KitchenCode),
                        methodArgument: models,
                        callerObject: GetType().Name);

                MaterialSpecificationItem? entity = entities.Find(x=> x.isUniqueKeyEqual(model));

                if(entity is null)
                    materialSpecificationItems.Add(
                        MaterialSpecificationItem.Create(
                            moduleType: moduleType,
                            materialSelectionItem: materialSelectionItem,
                            kitchen: kitchen));
                else
                    materialSpecificationItems.Add(
                        entity.Update(
                            moduleType: moduleType,
                            materialSelectionItem: materialSelectionItem,
                            kitchen: kitchen));
            }

            return materialSpecificationItems;
        }
    }
}
