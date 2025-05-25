using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
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

        public async Task<List<Section>> Convert(List<SectionDto> models, List<Section> entities)
        {
            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory
                    .GetRepository<Kitchen>()
                    .GetAllAsync(
                        predicate: 
                            x => models.Select(model => model.KitchenCode).Contains(x.Code)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => models.Select(model => model.ModuleCode).Contains(x.Code)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<Section> sections = [];

            foreach (SectionDto model in models)
            {
                var kitchen = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: kitchenResult.Find(
                        kitchen => kitchen.Code == model.KitchenCode),
                    methodArgument: models,
                    callerObject: GetType().Name);

                var module = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: moduleResult.Find(module => module.Code == model.ModuleCode),
                    methodArgument: models,
                    callerObject: GetType().Name);

                var quantity = model.Quantity;

                Section? entity = entities.Find(
                    x => x.Module.Code == model.ModuleCode 
                    && x.Kitchen.Code == model.KitchenCode);

                if(entity is null)
                    sections.Add(
                        Section.Create(
                            quantity: quantity,
                            kitchen: kitchen,
                            module: module));
                else
                    sections.Add(
                        entity.Update(
                            quantity: quantity,
                            kitchen: kitchen,
                            module: module));
            }

            return sections;
        }
    }
}
