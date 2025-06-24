using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public class ModelConverter : IDtoToEntityConverter<Model, ModelDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        public IDtoToEntityConverter<Model, ModelDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<Model, ModelDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<Model>> Convert(List<ModelDto> models, List<Model> entities)
        {
            var componentTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ComponentType>().GetAllAsync(predicate: x => models.Select(model => model.ComponentType).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<Model> ResultModels = [];

            foreach (ModelDto modelDto in models)
            {
                var componentType = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: componentTypeResult.Find(type => type.Title == modelDto.ComponentType),
                        methodArgument: models,
                        callerObject: GetType().Name);

                Model? model = entities.Find(c => c.IsUniqueKeyEqual(modelDto));

                if (model is null)
                    ResultModels.Add(
                        Model.Create(
                            title: modelDto.Title,
                            code: modelDto.Code,
                            componentType: componentType
                            )
                        );
                else
                    ResultModels.Add(
                        model.Update(
                            title: modelDto.Title,
                            code: modelDto.Code,
                            componentType: componentType
                            )
                        );
            }
            return ResultModels;
        }
    }
}
