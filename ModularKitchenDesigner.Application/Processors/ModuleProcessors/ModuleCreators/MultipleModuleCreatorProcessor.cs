using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.ModuleProcessors.ModuleCreators
{
    public class MultipleModuleCreatorProcessor : ICreatorProcessor<List<ModuleDto>, CollectionResult<ModuleDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<ModuleDto>, CollectionResult<ModuleDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<ModuleDto>, CollectionResult<ModuleDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<ModuleDto>> ProcessAsync(List<ModuleDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            var moduleResult = await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => data.Select(model => model.Code).Contains(x.Code));

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: moduleResult,
                    preffix: "",
                    suffix: suffix);

            var moduleTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => data.Select(model => model.Type).Contains(x.Title))).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            List<Module> moduleCreatorResult = await _repositoryFactory
                .GetRepository<Module>()
                .CreateMultipleAsync(
                    data.Select(model => new Module()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        ModuleTypeId = moduleTypeResult.Id,
                        PreviewImageSrc = model.PreviewImageSrc,
                        Width = model.Width,
                    }).ToList());

            var newModules = await _repositoryFactory
                .GetRepository<Module>()
                .GetAllAsync(
                    include: Module.IncludeRequaredField(),
                    predicate: x => data.Select(model => model.Code).Contains(x.Code));

            return new()
            {
                Count = newModules.Count,
                Data = newModules.Select(model => model.ConvertToDto())
            };
        }
    }
}
