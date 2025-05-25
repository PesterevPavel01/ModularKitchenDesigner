using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
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
        
        public async Task<List<Module>> Convert(List<ModuleDto> models, List<Module> entities)
        {
            var moduleTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => models.Select(model => model.Type ?? "default").Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<Module> modules = [];

            foreach (ModuleDto model in models)
            {
                var title = model.Title;
                var code = model.Code;
                var width = model.Width;
                var previewImageSrc = model.PreviewImageSrc;

                var moduleType = _validatorFactory
                    .GetObjectNullValidator()
                    .Validate(
                        model: moduleTypeResult.Find(x => x.Title == model.Type),
                    methodArgument: models,
                    callerObject: GetType().Name);

                Module? module = entities.Find(x => x.Code == model.Code);

                if (module is null)
                    modules.Add(
                        Module.Create(
                            title: title,
                            code: code,
                            previewImageSrc: previewImageSrc,
                            width: width,
                            moduleType: moduleType));
                else
                    modules.Add(
                        module.Update(
                            title: title,
                            code: code,
                            previewImageSrc: previewImageSrc,
                            width: width,
                            moduleType: moduleType));
            }

            return modules;
        }
    }
}
