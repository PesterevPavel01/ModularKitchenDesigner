using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;
using Module = ModularKitchenDesigner.Domain.Entityes.Module;

namespace ModularKitchenDesigner.Application.Processors.ModuleProcessors.ModuleCreators
{
    public sealed class SingleModuleCreatorProcessor : ICreatorProcessor<ModuleDto, BaseResult<ModuleDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        public ICreatorProcessor<ModuleDto, BaseResult<ModuleDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<ModuleDto, BaseResult<ModuleDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<ModuleDto>> ProcessAsync(ModuleDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var moduleResult = await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: moduleResult,
                    preffix: "",
                    suffix: suffix);

            var moduleTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<ModuleType>().GetAllAsync(predicate: x => x.Title == model.Type)).FirstOrDefault(),
                    preffix: "",
                   suffix: suffix);

            Module kitchenCreatorResult = await _repositoryFactory
                .GetRepository<Module>()
                .CreateAsync(
                    new Module()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        ModuleTypeId = moduleTypeResult.Id,
                        PreviewImageSrc = model.PreviewImageSrc,
                        Width = model.Width,
                    });

            var newModule = (await _repositoryFactory
                .GetRepository<Module>()
                .GetAllAsync(
                    include: Module.IncludeRequaredField(),
                    predicate: x => x.Code == model.Code
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newModule)
            };
        }

    }
}
