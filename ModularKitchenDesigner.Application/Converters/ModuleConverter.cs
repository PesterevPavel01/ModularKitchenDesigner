using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
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
        
        public async Task<List<Module>> Convert(List<ModuleDto> models, List<Module> entities, string[] validatorSuffix)
        {
            var moduleTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => models.Select(model => model.Type ?? "default").Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            foreach (ModuleDto model in models)
            {
                Module? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(entity => model.Code == entity.Code),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.Width = model.Width;
                entity.PreviewImageSrc = model.PreviewImageSrc;
                entity.ModuleTypeId = moduleTypeResult.Where(type => type.Title == model.Type).FirstOrDefault().Id;
            }

            return entities;
        }
    }
}
