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
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<Model, ModelDto>, Model, ModelDto>()
                .ProcessAsync(predicate:
                    x => x.Enabled == true
                    && modelItemsResult.Data.Select(item => item.ModelCode).Contains(x.Code));

            var modelsStruct =
                (from modelItems in modelItemsResult.Data
                 join models in modelsResult.Data
                     on modelItems.ModelCode equals models.Code
                 select new
                 {
                     models.ComponentType,
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
                     models.Code,
                     models.ComponentType
                 }).ToList();

            //Строю реальную структуру кухни
            //Получаю Материалы, из которых строится кухня.
            //ModulType -> ComponentType -> Material -> PriceSegment(справочно)

            var kitchenMaterialSpecificationItems = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSpecificationItem, MaterialSpecificationItemDto>, MaterialSpecificationItem, MaterialSpecificationItemDto>()
                .ProcessAsync(predicate:
                    x => x.Kitchen.Code == model.KitchenCode);

            var MaterialSelectionItems = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<MaterialSelectionItem, MaterialSelectionItemDto>, MaterialSelectionItem, MaterialSelectionItemDto>()
                .ProcessAsync(predicate:
                    x => kitchenMaterialSpecificationItems.Data.Select(item => item.MaterialSelectionItemCode).Contains(x.Code));

            var kitchenMaterialSelectionItems = kitchenMaterialSpecificationItems.Data
                .Select(item => new
                {
                    item.ModuleType,
                    MaterialSelectionItem = MaterialSelectionItems.Data.First(x => x.Code == item.MaterialSelectionItemCode)
                }).ToList();

            var ModuleTypes = kitchenMaterialSelectionItems.GroupBy(x => x.ModuleType).Select(x => x.First().ModuleType).ToList();
            
            List<SpecificationItem> resultListComponents = [];
            
            foreach (var item in ModuleTypes)
            {
                //отдельно верх, отдельно низ
                var currentMaterialSelectionItem = kitchenMaterialSelectionItems.Where(selItem => selItem.ModuleType == item).ToList();
                var componentResult = await _processorFactory
                    .GetLoaderProcessor<CommonDefaultLoaderProcessor<Component, ComponentDto>, Component, ComponentDto>()
                    .ProcessAsync(predicate: x =>
                        x.Enabled == true &&
                        (x.Material.Title == "default" || currentMaterialSelectionItem.Select(item => item.MaterialSelectionItem.Material).Contains(x.Material.Title)) &&
                        (x.PriceSegment.Title == "default" || kitchenTypeResult.Data.First().PriceSegment == x.PriceSegment.Title) &&
                        currentModels.Select(model => model.Code).Contains(x.Model.Code));

                //переношу реальные компоненты на абстрактную структуру
                resultListComponents.AddRange(
                (from models in currentModels.Where(x => x.ModuleType == item).ToList()
                 join components in componentResult.Data
                     on models.Title equals components.Model
                 group new { models, components } by components.Code into g
                 where g.Any() // проверка на пустые группы
                 select new SpecificationItem
                 {
                     Code = g.Key,
                     Title = g.First().components.Title ?? string.Empty,
                     Quantity = g.Sum(x => x.models.Quantity),
                     UnitPrice = g.First().components.Price, // или Average
                     Model = g.First().components.Model,
                     ComponentType = g.First().models.ComponentType,
                     Material = g.First().components.Material ?? string.Empty,
                     TotalPrice = g.Sum(x => x.models.Quantity * x.components.Price)
                 }).Where(x => x != null)// дополнительная фильтрация
            );
            }

            return new()
            {
                Data = new()
                {
                    KitchenTitle = kitchenResult.Data.First().Title,
                    KitchenCode = kitchenResult.Data.First().Code,
                    Price = resultListComponents.Sum(x => x.TotalPrice),
                    Width = maxWidth,
                    UserCode = kitchenResult.Data.First().UserLogin,
                    UserId = kitchenResult.Data.First().UserId,
                    Specification = resultListComponents.OrderBy(x => x.ComponentType).ToList(),
                }
            };

        }

    }
}
