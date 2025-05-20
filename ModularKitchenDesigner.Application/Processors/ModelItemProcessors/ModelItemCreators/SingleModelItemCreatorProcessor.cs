using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Processors.ModelItemProcessors.ModelItemCreators
{
    public sealed class SingleModelItemCreatorProcessor : ICreatorProcessor<ModelItemDto, BaseResult<ModelItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        public ICreatorProcessor<ModelItemDto, BaseResult<ModelItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<ModelItemDto, BaseResult<ModelItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<ModelItemDto>> ProcessAsync(ModelItemDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var materialItemResult = await _repositoryFactory
               .GetRepository<ModelItem>()
               .GetAllAsync(predicate:
                   x => x.Module.Code == model.ModuleCode
                   && x.Model.Code == model.ModelCode);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: materialItemResult,
                    preffix: "",
                    suffix: suffix);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => x.Code == model.ModuleCode)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => x.Code == model.ModelCode)).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            ModelItem modelItemCreatorResult = await _repositoryFactory
                .GetRepository<ModelItem>()
                .CreateAsync(
                    new ModelItem()
                    {
                        ModuleId = moduleResult.Id,
                        ModelId = modelResult.Id,
                        Quantity = model.Quantity
                    });

            var newModelItem = (await _repositoryFactory
                .GetRepository<ModelItem>()
                .GetAllAsync(
                    include: ModelItem.IncludeRequaredField(),
                    predicate:
                        x => x.Module.Code == model.ModuleCode
                        && x.Model.Code == model.ModelCode))
                .FirstOrDefault();

            return new()
            {
                Data = new(newModelItem)
            };
        }

    }
}
