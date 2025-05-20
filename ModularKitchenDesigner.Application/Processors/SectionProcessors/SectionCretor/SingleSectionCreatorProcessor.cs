using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SectionProcessors.SectionCretor
{
    public class SingleSectionCreatorProcessor : ICreatorProcessor<SectionDto, BaseResult<SectionDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;


        public ICreatorProcessor<SectionDto, BaseResult<SectionDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<SectionDto, BaseResult<SectionDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<BaseResult<SectionDto>> ProcessAsync(SectionDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Section>().GetAllAsync(
                        predicate: x => x.Kitchen.Id == model.KitchenGuid
                        && x.Module.Code == model.ModuleCode),
                    preffix: "",
                    suffix: suffix);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => x.Code == model.ModuleCode)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => x.Id == model.KitchenGuid)).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            Section section = await _repositoryFactory
                .GetRepository<Section>()
                .CreateAsync(
                    new Section()
                    {
                        ModuleId = moduleResult.Id,
                        KitchenId = kitchenResult.Id,
                        Quantity = model.Quantity,
                    });

            var newSection = (await _repositoryFactory
                .GetRepository<Section>()
                .GetAllAsync(
                    include: Section.IncludeRequaredField(),
                    predicate: x => x.Kitchen.Id == model.KitchenGuid
                    && x.Module.Code == model.ModuleCode))
                .FirstOrDefault();

            return new()
            {
                Data = new(newSection)
            };
        }

    }
}
