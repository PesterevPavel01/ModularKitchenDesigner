using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Exchange.Interpreter;
using ModularKitchenDesigner.Application.Exchange.Interpritators;
using ModularKitchenDesigner.Application.Exchange.Processors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Services
{
    public class ExchengeWith1СService : IExchangeService<NomanclatureDto>
    {
        private readonly IProcessorFactory _processorFactory;
        private readonly MaterialSelectionItemInterpreter _materialSelectionItemInterpreter;
        private readonly ModelItemInterpreter _modelItemInterpreter;
        private readonly ComponentInterpreter _componentInterpreter;
        private readonly RulesProcessor _exchangeRulesProcessor;

        public ExchengeWith1СService(IProcessorFactory processorFactory, MaterialSelectionItemInterpreter materialSelectionItemInterpreter,
            ModelItemInterpreter modelItemInterpreter, RulesProcessor exchangeRulesProcessor, ComponentInterpreter componentInterpreter)
        {
            _componentInterpreter = componentInterpreter;
            _processorFactory = processorFactory;
            _materialSelectionItemInterpreter = materialSelectionItemInterpreter;
            _modelItemInterpreter = modelItemInterpreter;
            _exchangeRulesProcessor = exchangeRulesProcessor;
        }
        public async Task<CollectionResult<NomanclatureDto>> ExchangeAsync(List<NomanclatureDto> models)
        {
            if (models is null || models.Count < 1)
                return new();

            var componentTypeExchangeProcessor = await ProcessAsync<ComponentType, SimpleDto, SimpleEntityConverter< ComponentType>>(models);

            var materialExchangeProcessor = await ProcessAsync<Material, SimpleDto, SimpleEntityConverter<Material>>(models);

            var modelExchangeProcessor = await ProcessAsync<Model, ModelDto, ModelConverter>(models);

            var moduleTypeExchangeProcessor = await ProcessAsync<ModuleType, SimpleDto, SimpleEntityConverter<ModuleType>>(models);

            var priceSegmentExchangeProcessor = await ProcessAsync<PriceSegment, SimpleDto, SimpleEntityConverter<PriceSegment>>(models);

            //Свой метод определения по шаблону
            var componentModels = await _componentInterpreter.InterpretAsync(models);

            if(componentModels.Data is not null)
                await new ExchangeProcessor<Component, ComponentDto, ComponentConverter>()
                    .SetProcessorFactory(_processorFactory)
                    .ProcessAsync([.. componentModels.Data], x => x.Template is not null);

            var kitchenTypeExchangeProcessor = await ProcessAsync<KitchenType, KitchenTypeDto, KitchenTypeConverter>(models);
            
            var moduleExchangeProcessor = await ProcessAsync<Module, ModuleDto, ModuleConverter>(models);

            //т.к. в системе, реализованной в 1с, не реализована возможность добавления одного и того же материала в разные виды кухонь, что неправильно (является серьезным недочетом, который необходимо исправить),
            //а в модели приложения эта возможность реализована, необходимо предварительно пропустить пришедший Material через MaterialSelectionItemAdapter 

            var materialSelectionItemModels = await _materialSelectionItemInterpreter.InterpretAsync(models);

            var materialSelectionItemExchangeProcessor = await ProcessAsync<MaterialSelectionItem, MaterialSelectionItemDto, MaterialSelectionItemConverter>([.. materialSelectionItemModels.Data]);

            // чтобы обновить modelItems, которые содержатся в списке 
            // нужно сделать чтобы у обновляемых модулей все существующие ModelItem стали Enabled = false
            // не удаляет неактивные modelItems
            var modelRules = _exchangeRulesProcessor.GetModelRules<ModelItem>();

            var modelItemExchangeModels = await _modelItemInterpreter
                .InterpretAsync( 
                    models
                        .Where(modelItem => modelItem.Parents is not null && modelRules
                            .Any(rule =>
                                (rule.Limit == 0 || modelItem.Parents.Count == rule.Limit)
                                && modelItem.Parents.FindIndex(p => p.Code == rule.Code) == rule.Parent)).ToList());
            
            if(modelItemExchangeModels.Data is not null && modelItemExchangeModels.Count > 0)
                await ProcessAsync<ModelItem, ModelItemDto, ModelItemConverter>([.. modelItemExchangeModels.Data], x => x.Models is not null);

            return new() 
            {
                Count = models != null ? models.Count : 0,
                //Data = models
            };
        }

        private async Task<CollectionResult<NomanclatureDto>> ProcessAsync<TEntity, TDto, TConverter>(List<NomanclatureDto> models, Func<NomanclatureDto, bool>? predicat = null)
            where TDto : BaseDto, IExcangeDtoConvertable<TDto, NomanclatureDto>, IUniqueKeyQueryable<TDto>, new()
            where TEntity : BaseEntity, IDtoConvertible<TEntity, TDto>
            where TConverter : IDtoToEntityConverter<TEntity, TDto>, new()
        {

            var modelRules = _exchangeRulesProcessor.GetModelRules<TEntity>();

            return await new ExchangeProcessor<TEntity, TDto, TConverter>()
                .SetProcessorFactory(_processorFactory)
                .ProcessAsync(models, x => x.Parents is not null && modelRules
                    .Any(rule =>
                        (predicat is null || predicat(x))
                        && (rule.Limit == 0 || x.Parents.Count == rule.Limit)
                        && x.Parents.FindIndex(p => p.Code == rule.Code) == rule.Parent
                        && (!rule.Models || (x.Models is not null && x.Models.Count != 0)) // если в правилах указано обязательное наличие моделей
                        && (!rule.Folder || (x.Price is null && x.Widht is null)) // если в правилах указано обязательное наличие моделей
                        ));
        }
    }
}
