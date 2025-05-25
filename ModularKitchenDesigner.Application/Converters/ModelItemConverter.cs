using System.Collections.Generic;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class ModelItemConverter : IDtoToEntityConverter<ModelItem, ModelItemDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<ModelItem, ModelItemDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<ModelItem, ModelItemDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<List<ModelItem>> Convert(List<ModelItemDto> models, List<ModelItem> entities)
        {
            var moduleResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                model: await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => models.Select(model => model.ModuleCode).Contains(x.Code)),
                methodArgument: models,
                callerObject: GetType().Name);

            var modelResult = _validatorFactory
            .GetObjectNullValidator()
            .Validate(
                model: await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => models.Select(model => model.ModelCode).Contains(x.Code)),
                methodArgument: models,
                callerObject: GetType().Name);

            List < ModelItem > modelItems = [];

            foreach (ModelItemDto modelItem in models)
            {
                var module = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: moduleResult.Find(x => x.Code == modelItem.ModuleCode),
                methodArgument: models,
                callerObject: GetType().Name);

                var model =_validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: modelResult.Find(x => x.Code == modelItem.ModelCode),
                    methodArgument: models, callerObject: GetType().Name);

                var quantity = modelItem.Quantity;

                ModelItem? entity = entities.Find(
                    x => x.Module.Code == modelItem.ModuleCode
                    && x.Model.Code == modelItem.ModelCode);

                if(entity is null)
                    modelItems.Add(
                        ModelItem.Create(
                            quantity: quantity,
                            module: module,
                            model: model));
                else
                    modelItems.Add(
                        entity.Update(
                            quantity: quantity,
                            module: module,
                            model: model));
            }

            return modelItems;
        }

    }
}
