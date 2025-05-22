using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public class ModuleConverter : IDtoToEntityConverter<Module, ModuleDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<Module, ModuleDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<Module, ModuleDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        
        public async Task<List<Module>> Convert(List<ModuleDto> models, List<Module> entities, Func<ModuleDto, Func<Module, bool>> findEntityByDto)
        {
            var moduleTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => models.Select(model => model.Type ?? "default").Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            foreach (ModuleDto model in models)
            {
                Module? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.Width = model.Width;
                entity.PreviewImageSrc = model.PreviewImageSrc;

                entity.ModuleTypeId = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: moduleTypeResult.Find(x => x.Title == model.Type),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.ModuleTypeId;
            }

            return entities;
        }
    }
}
