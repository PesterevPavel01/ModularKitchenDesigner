using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SectionProcessors.SectionCretor
{
    public class MultipleSectionCreatorProcessor : ICreatorProcessor<List<SectionDto>, CollectionResult<SectionDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<SectionDto>, CollectionResult<SectionDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<SectionDto>, CollectionResult<SectionDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<SectionDto>> ProcessAsync(List<SectionDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Section>().GetAllAsync(
                        predicate:
                            x => data.Select(model => model.KitchenGuid).Contains(x.Kitchen.Id)
                            && data.Select(model => model.ModuleCode).Contains(x.Module.Code)),
                    preffix: "",
                    suffix: suffix);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => data.Select(model => model.ModuleCode).Contains(x.Code))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var kitchenResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Kitchen>().GetAllAsync(predicate: x => data.Select(model => model.KitchenGuid).Contains(x.Id))).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            List<Section> sections = await _repositoryFactory
                .GetRepository<Section>()
                .CreateMultipleAsync(
                data.Select(model => new Section()
                    {
                        ModuleId = moduleResult.Id,
                        KitchenId = kitchenResult.Id,
                        Quantity = model.Quantity,
                    }).ToList());

            var newSections = await _repositoryFactory
                .GetRepository<Section>()
                .GetAllAsync(
                    include: Section.IncludeRequaredField(),
                    predicate: x => data.Select(model => model.KitchenGuid).Contains(x.Kitchen.Id)
                            && data.Select(model => model.ModuleCode).Contains(x.Module.Code));

            return new()
            {
                Count = newSections.Count,
                Data = newSections.Select(x => x.ConvertToDto())
            };
        }
    }
}
