using ModularKitchenDesigner.Application.Exchange.Processors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Interpritators
{
    public class ModelItemInterpreter
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private readonly RulesProcessor _exchangeRulesProcessor;

        public ModelItemInterpreter(IRepositoryFactory repositoryFactory, RulesProcessor exchangeRulesProcessor)
        {
            _repositoryFactory = repositoryFactory;
            _exchangeRulesProcessor = exchangeRulesProcessor;
        }

        public async Task<CollectionResult<NomanclatureDto>> InterpretAsync(List<NomanclatureDto> externalModels)
        {
            if (externalModels is null || externalModels.Count < 1)
                return new();

            var result = externalModels
                .SelectMany(x => x.Models == null
                    ? []
                    : x.Models.Select(model =>
                        new NomanclatureDto()
                        {
                            Code = x.Code,
                            Models = [model],
                            Parents = x.Parents,
                            Title = model.Title != "removed" ? x.Title : "removed"
                        }))
                .ToList();

            var existingEntities = await _repositoryFactory
                .GetRepository<ModelItem>()
                    .GetAllAsync(
                        include: ModelItem.IncludeRequaredField(),
                        predicate: x => externalModels.Select(model => model.Code).Contains(x.Module.Code));

            // если в Models не будет какой-то модели, которая в системе привязана к модулю через ModelItem, то существующая модель будет включена в входной пакет моделей с пометкой удаления "removed" 
            // для установки у нее флага Enabled = false при дальнейшем переносе данных

            if (existingEntities.Count > 0)
            {
                var modelRules = _exchangeRulesProcessor.GetModelRules<ModelItem>();

                var removedEntities = existingEntities
                    .Where(model => result.FirstOrDefault(x => x.Code == model.Module.Code && x.Models.Count > 0 && x.Models[0].Code == model.Model.Code) is null).ToList();

                var removedModels = removedEntities
                    .Select(model => new NomanclatureDto
                    {
                        Title = model.Module.Title,
                        Code = model.Module.Code,
                        Models =
                        [
                            new()
                            {
                                Code = model.Model.Code,
                                Title = "removed",
                            }
                        ],
                        Parents = [.. Enumerable.Range(0, modelRules.First().Parent + 1)
                            .Select(item => new SimpleDto()
                            {
                                Code = modelRules.FirstOrDefault(rule => rule.Parent == item)?.Code ?? string.Empty
                            })]
                    }).ToList();

                result.AddRange(removedModels);
            }

            return
            new()
            {
                Count = result.Count,
                Data = result
            };
        }
    }
}
