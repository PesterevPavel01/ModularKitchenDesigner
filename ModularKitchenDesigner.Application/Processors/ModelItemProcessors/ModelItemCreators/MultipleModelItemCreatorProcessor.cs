using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.ModelItemProcessors.ModelItemCreators
{
    public class MultipleModelItemCreatorProcessor : ICreatorProcessor<List<ModelItemDto>, CollectionResult<ModelItemDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<ModelItemDto>, CollectionResult<ModelItemDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<ModelItemDto>, CollectionResult<ModelItemDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<ModelItemDto>> ProcessAsync(List<ModelItemDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            var materialItemResult = await _repositoryFactory
               .GetRepository<ModelItem>()
               .GetAllAsync(predicate:
                    x => data.Select(model => model.ModuleCode).Contains(x.Module.Code)
                    && data.Select(model => model.ModelCode).Contains(x.Model.Code));

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: materialItemResult,
                    preffix: "",
                    suffix: suffix);

            var moduleResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => data.Select(model => model.ModuleCode).Contains(x.Code))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            var modelResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Model>().GetAllAsync(predicate: x => data.Select(model => model.ModelCode).Contains(x.Code))).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);

            List<ModelItem> modelItemCreatorResult = await _repositoryFactory
                .GetRepository<ModelItem>()
                .CreateMultipleAsync(
                data.Select(model => new ModelItem()
                    {
                        ModuleId = moduleResult.Id,
                        ModelId = modelResult.Id,
                        Quantity = model.Quantity
                    }).ToList());

            var newModelItems = await _repositoryFactory
                .GetRepository<ModelItem>()
                .GetAllAsync(
                    include: ModelItem.IncludeRequaredField(),
                    predicate:
                        x => data.Select(model => model.ModuleCode).Contains(x.Module.Code)
                        && data.Select(model => model.ModelCode).Contains(x.Model.Code));

            return new()
            {
                Count = newModelItems.Count,
                Data = newModelItems.Select(model => model.ConvertToDto())
            };
        }

    }
}
