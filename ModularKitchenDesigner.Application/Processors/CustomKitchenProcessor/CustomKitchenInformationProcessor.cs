using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Kustom;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using Result;

namespace ModularKitchenDesigner.Application.Processors.CustomKitchenProcessor
{
    public class CustomKitchenInformationProcessor
    {
        private readonly IProcessorFactory _processorFactory = null!;

        public CustomKitchenInformationProcessor(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public async Task<BaseResult<KustomKitchenDto>> ProcessAsync(KustomKitchenDto model)
        {

            var kitchenResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Kitchen, KitchenDto>, Kitchen, KitchenDto>()
                .ProcessAsync(predicate:
                    x => x.Code == model.KitchenCode);

            var kitchenTypeResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<KitchenType, KitchenTypeDto>, KitchenType, KitchenTypeDto>()
                .ProcessAsync(predicate:
                    x => x.Enabled == true && x.Title == kitchenResult.Data.First().KitchenType);

            //загружаю секции, из которых состоит кухня
            var sectionsResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Section, SectionDto>, Section, SectionDto>()
                .ProcessAsync(predicate: 
                        x => x.Kitchen.Code == model.KitchenCode);

            //загружаю модули, из которых состоят секции
            var modulesResult = await _processorFactory
            .GetLoaderProcessor<CommonDefaultLoaderProcessor<Module, ModuleDto>, Module, ModuleDto>()
                .ProcessAsync(predicate: 
                        x => x.Enabled == true && sectionsResult.Data.Select(section => section.ModuleCode).Contains(x.Code));

            var currentSectionModuleStructure =
                (from modules in modulesResult.Data
                 join sections in sectionsResult.Data
                    on modules.Code equals sections.ModuleCode
                 select new
                 {
                     modules.Code,
                     modules.Width,
                     sections.Quantity,
                     SectionsWidth = sections.Quantity * modules.Width,
                     ModuleType = modules.Type,
                 }).ToList();

            //Считаю ширину верхней и нижней части
            var totalWidth = currentSectionModuleStructure.GroupBy(x => x.ModuleType).Select(x => new { x.First().ModuleType, TotalWidth = x.Sum(section => section.SectionsWidth) }).ToList();

            var maxWidth = totalWidth.Max(x => x.TotalWidth);

            model.Width = maxWidth;

            var modelItemsResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<ModelItem, ModelItemDto>, ModelItem, ModelItemDto>()
                .ProcessAsync(predicate: x => x.Enabled == true && currentSectionModuleStructure.Select(item => item.Code).Contains(x.Module.Code));

            var modelsResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Model, SimpleDto>, Model, SimpleDto>()
                .ProcessAsync(predicate:
                    x => x.Enabled == true
                    && modelItemsResult.Data.Select(item => item.ModelCode).Contains(x.Code));

            var modelsStruct =
                (from modelItems in modelItemsResult.Data
                join models in modelsResult.Data
                    on modelItems.ModelCode equals models.Code
                select new 
                {
                    models.Title,
                    models.Code,
                    modelItems.Quantity,
                    modelItems.ModuleCode
                }).ToList();


            var currentModels =
                (from modules in currentSectionModuleStructure
                join models in modelsStruct
                    on modules.Code equals models.ModuleCode
                 select new
                 {
                     ModuleCode = modules.Code,
                     modules.ModuleType,
                     Quantity = modules.Quantity * models.Quantity,
                     models.Title,
                     Code = models.Code
                 }).ToList();

            // Строю реальную структуру кухни
            //Получаю Материалы, из которых строится кухня.
            //ModulType -> ComponentType -> Material -> PriceSegment(справочно)

            var kitchenMaterialSpecificationItems = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>, MaterialSpecificationItem, MaterialSpecificationItemDto>()
                .ProcessAsync(predicate: 
                    x => x.Kitchen.Code == model.KitchenCode);

            var kitchenMaterialSelectionItems = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>()
                .ProcessAsync( predicate: 
                    x => kitchenMaterialSpecificationItems.Data.Select(item => item.MaterialSelectionItemCode).Contains(x.Code));

            var componentResult = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Component, ComponentDto>, Component, ComponentDto>()
                .ProcessAsync(predicate:
                    x => x.Enabled == true
                    && x.Material.Title == "default" ? true : kitchenMaterialSelectionItems.Data.Select(item => item.Material).Contains(x.Material.Code)
                    && x.PriceSegment.Title == "default" ? true : kitchenTypeResult.Data.First().PriceSegment == x.PriceSegment.Title
                    && currentModels.Select(model => model.Code).Contains(x.Model.Code));

            //переношу реальные компоненты на абстрактную структуру
            var resultListComponents = (
                from models in currentModels
                join components in componentResult.Data
                    on models.Title equals components.Model
                select new
                {
                    components.Code,
                    models.Quantity,
                    components.Price,
                    TotalPrice = models.Quantity * components.Price,
                    ModelTitle = components.Model,
                    components.PriceSegment,
                    components.Material
                }
                ).ToList();

            return new()
            {
                Data = new()
                {
                    KitchenTitle = kitchenResult.Data.First().Title,
                    KitchenCode = kitchenResult.Data.First().Code,
                    Price = resultListComponents.Sum(x => x.TotalPrice),
                    Width = maxWidth,
                    UserCode = kitchenResult.Data.First().UserLogin,
                    UserId = kitchenResult.Data.First().UserId
                }
            };

        }

    }
}
