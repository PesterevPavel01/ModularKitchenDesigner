using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class SectionConverter : IDtoToEntityConverter<Section, SectionDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<Section, SectionDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<Section, SectionDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<Section>> Convert(List<SectionDto> models, List<Section> entities, string[] validatorSuffix)
        {
            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => models.Select(model => model.KitchenGuid).Contains(x.Id)),
                    preffix: "",
                    suffix: validatorSuffix);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => models.Select(model => model.ModuleCode).Contains(x.Code)),
                    preffix: "",
                    suffix: validatorSuffix);

            foreach (SectionDto model in models)
            {
                Section? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(entity => model.ModuleCode == entity.Module.Code && model.KitchenGuid == entity.Kitchen.Id),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.Quantity = model.Quantity;
            }

            return entities;
        }
    }
}
