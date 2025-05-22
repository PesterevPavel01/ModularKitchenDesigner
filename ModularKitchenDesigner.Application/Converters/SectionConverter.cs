using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
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

        public async Task<List<Section>> Convert(List<SectionDto> models, List<Section> entities, Func<SectionDto, Func<Section, bool>> findEntityByDto)
        {
            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => models.Select(model => model.KitchenGuid).Contains(x.Id)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => models.Select(model => model.ModuleCode).Contains(x.Code)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            foreach (SectionDto model in models)
            {
                Section? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.KitchenId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: kitchenResult.Find(kitchen => kitchen.Id == model.KitchenGuid),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.KitchenId;

                entity.ModuleId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: moduleResult.Find(module => module.Code == model.ModuleCode),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.ModuleId;

                entity.Quantity = model.Quantity;
            }

            return entities;
        }
    }
}
