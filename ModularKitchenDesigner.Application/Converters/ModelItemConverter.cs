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
        public async Task<List<ModelItem>> Convert(List<ModelItemDto> models, List<ModelItem> entities, Func<ModelItemDto, Func<ModelItem, bool>> findEntityByDto)
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

            foreach (ModelItemDto model in models)
            {
                ModelItem? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.ModuleId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: moduleResult.Find(x => x.Code == model.ModuleCode),
                methodArgument: models,
                callerObject: GetType().Name)?.Id ?? entity.ModuleId;

                entity.ModelId =_validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: modelResult.Find(x => x.Code == model.ModelCode),
                    methodArgument: models, callerObject: GetType().Name)?.Id ?? entity.ModelId;

                entity.Quantity = model.Quantity;
            }

            return entities;
        }

    }
}
