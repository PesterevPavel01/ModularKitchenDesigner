using System.Collections.Concurrent;
using ModularKitchenDesigner.Application.Exchange.Processors;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Interpreter
{
    public class ComponentInterpreter
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private readonly RulesProcessor _exchangeRulesProcessor;

        public ComponentInterpreter(IRepositoryFactory repositoryFactory, RulesProcessor exchangeRulesProcessor)
        {
            _repositoryFactory = repositoryFactory;
            _exchangeRulesProcessor = exchangeRulesProcessor;
        }

        public async Task<CollectionResult<NomanclatureDto>> InterpretAsync(List<NomanclatureDto> externalModels)
        {
            
            List<NomanclatureDto> createOrUpdateComponents = [.. externalModels.Where(model => model.Template is not null && !string.IsNullOrEmpty(model.Template.Code))];

            var result = await GetCreateOrUpdateComponentAsync(createOrUpdateComponents);

            var notComponentModels = externalModels.Where(model => model.Template == null || string.IsNullOrWhiteSpace(model.Template.Code)).ToList();

            result.AddRange(await GetRemovedComponentAsync(notComponentModels));

            return
                new()
                {
                    Count = result.Count,
                    Data = result
                };
        }

        async Task<List<NomanclatureDto>> GetRemovedComponentAsync(List<NomanclatureDto> notComponentModels)
        {
            if (notComponentModels.Any())
            {
                var removedComponent = (await _repositoryFactory
                        .GetRepository<Component>()
                            .GetAllAsync(
                                include: Component.IncludeRequaredField(),
                                predicate: x => notComponentModels.Select(model => model.Code).Contains(x.Code)))
                    ?.Select(component => new NomanclatureDto() { Code = component.Code, Title = "removed" });

                if (removedComponent is not null)
                    return [.. removedComponent];
            }

            return [];
        }

            async Task<List<NomanclatureDto>> GetCreateOrUpdateComponentAsync(List<NomanclatureDto> createOrUpdateComponents) 
        {
            if (createOrUpdateComponents is null || createOrUpdateComponents.Count < 1)
                return new();

            var templateResult = await _repositoryFactory
                .GetRepository<Model>()
                    .GetAllAsync(
                        include: Model.IncludeRequaredField(),
                        predicate: x => createOrUpdateComponents.Select(model => model.Template.Code).Contains(x.Code));

            // Создаем список для сбора ошибок (потокобезопасный)
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(createOrUpdateComponents, model =>
            {
                try
                {
                    var componentTypeCode = templateResult?
                        .Find(x => x.Code == model.Template.Code)?.ComponentType.Code;

                    if (componentTypeCode is null)
                        throw new InvalidOperationException(
                            $"У модели (Code: {model.Code}), определенной как Component, " +
                            $"указан незарегистрированный в системе Шаблон (Code: {model.Template.Code})");

                    var paramters = _exchangeRulesProcessor.GetParametrizeComponentRules(componentTypeCode);

                    string material = "default";
                    string priceSegment = "default";

                    if (paramters is not null)
                    {
                        if (paramters.TryGetValue("Material", out int materialValue))
                        {
                            if (model.Parents is null || !(model.Parents.Count > materialValue))
                                throw new InvalidOperationException(
                                    $"Модель (Code: {model.Code}), определенная как Component, " +
                                    $"не содержит требуемых параметров (Parents), указанных в ExchangeRules");

                            material = model.Parents[materialValue].Title;
                        }

                        if (paramters.TryGetValue("PriceSegment", out int priceSegmentValue))
                        {
                            if (model.Parents is null || !(model.Parents.Count > priceSegmentValue))
                                throw new InvalidOperationException(
                                    $"Модель (Code: {model.Code}), определенная как Component, " +
                                    $"не содержит требуемых параметров (Parents), указанных в ExchangeRules");

                            priceSegment = model.Parents[priceSegmentValue].Title;
                        }
                    }

                    // Потокобезопасное обновление модели
                    lock (model)
                    {
                        model.Parents = [
                            new(){ Title = material},
                            new(){ Title = priceSegment}
                        ];
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException("Ошибки при обработке моделей", exceptions);
            }

            return createOrUpdateComponents;
        }
    }
}
