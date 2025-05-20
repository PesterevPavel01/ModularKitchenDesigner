using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.OrdersProcessors
{
    public sealed class OrderCreatorProcessor : ICreatorProcessor<OrderDto, BaseResult<OrderDto>>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<OrderDto, BaseResult<OrderDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public ICreatorProcessor<OrderDto, BaseResult<OrderDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public async Task<BaseResult<OrderDto>> ProcessAsync(OrderDto model)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var kitchenTypeResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(
                        include: query => query.Include(x => x.PriceSegment),
                        predicate: x => x.Code == model.KitchenTypeCode,
                        trackingType: TrackingType.NoTracking
                        )).FirstOrDefault(),
                    preffix: "",
                    suffix: suffix);


            List<string> moduleCodes = model.Sections.Select(x => x.ModuleCode).ToList();

            ///Строю абстрактную структуру кухни из моделей компонентов
            ///
            
            var currentModules = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Module>().GetAllAsync(
                        include: query => query.Include(x => x.Type),
                        predicate: x => moduleCodes.Contains(x.Code),
                        trackingType: TrackingType.NoTracking),
                    preffix: "",
                    suffix: suffix);
            
            //добавляю к списку модулей их количество, поступившее от пользователя
            var currentSectionModuleStructure = 
                (from modules in currentModules
                join sections in model.Sections
                on modules.Code equals sections.ModuleCode 
                select new 
                { 
                    modules.Code,
                    modules.Width,
                    sections.Quantity,
                    SectionsWidth = sections.Quantity * modules.Width,
                    ModuleType = modules.Type.Title,
                }).ToList();

            //Считаю ширину верхней и нижней части
            var totalWidth = currentSectionModuleStructure.GroupBy(x => x.ModuleType).Select(x => new { x.First().ModuleType, TotalWidth = x.Sum(section => section.SectionsWidth)}).ToList() ;

            var maxWidth = totalWidth.Max(x => x.TotalWidth);

            //получаю модели, которые входят в структуру модулей
            var currentModulComponentModelStructure = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<ModelItem>().GetAllAsync(
                        include: query => query.Include(x => x.Module).Include(x => x.Model),
                        predicate: x => moduleCodes.Contains(x.Module.Code),
                        trackingType: TrackingType.NoTracking
                        ),
                    preffix: "",
                    suffix: suffix);

            var currentModels = 
                (from sections in currentSectionModuleStructure
                    join models in currentModulComponentModelStructure
                    on sections.Code equals models.Module.Code
                    select new
                    {
                        ModuleCode = sections.Code,
                        sections.ModuleType,
                        Quantity = sections.Quantity * models.Quantity,
                        ModelCode = models.Model.Code
                    }).ToList();

            /// Строю реальную структуру кухни
            /// 

            //Получаю Материалы, из которых строится кухня.
            //ModulType -> ComponentType -> Material -> PriceSegment(справочно)
            
            List<MaterialSelectionItem> kitchenMaterialItems = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                 models: await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(
                     include: query => query.Include(x => x.Material).Include(x => x.ComponentType).Include(x => x.KitchenType),
                     predicate: x => x.KitchenType.Code == model.KitchenTypeCode,
                     trackingType: TrackingType.NoTracking),
                 preffix: "",
                 suffix: suffix);


            var componentMaterials = kitchenMaterialItems
                .Select( x => new 
                { 
                    x.MaterialId,
                    x.ComponentTypeId,
                    kitchenTypeResult.PriceSegmentId

                }).ToList();

            // Беру компоненты, без учета ценового сегмента
            
            List<Component> componens = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                models: await _repositoryFactory.GetRepository<Component>().GetAllAsync(
                     include: query => query.Include( x => x.Model).Include( x => x.ComponentType).Include(x => x.PriceSegment).Include(x => x.Material),
                     predicate: x => componentMaterials.Select(cd => cd.MaterialId).Contains(x.MaterialId)
                        && componentMaterials.Select(cd => cd.ComponentTypeId).Contains(x.ComponentTypeId),
                     trackingType: TrackingType.NoTracking),
                 preffix: "",
                 suffix: suffix);

            //фильтрую компоненты по ценовому сегменту, если он не default

            List<Component> componentsFilterByPriceSegment = componens.Where(x => x.PriceSegment.Id == kitchenTypeResult.PriceSegmentId || x.PriceSegment.Title == "default").ToList();

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------

            //переношу реальные компоненты на абстрактную структуру
            var resultListComponents = (
                from models in currentModels
                join components in componens
                    on models.ModelCode equals components.Model.Code
                    select new 
                    {
                        components.Code,
                        models.Quantity,
                        components.Price,
                        TotalPrice = models.Quantity * components.Price,
                        ModelCode = components.Model.Code,
                        Model = components.Model.Title,
                        PriceSegment = components.PriceSegment.Title,
                        Material = components.Material.Title,
                    }
                ).ToList();

            return new()
            {
                Data = new()
                {
                    KitchenTitle = "Модульная кухня",
                    KitchenType = kitchenTypeResult.Title,
                    KitchenTypeCode = kitchenTypeResult.Code,
                    Sections = model.Sections,
                    Price = resultListComponents.Sum(x => x.TotalPrice),
                    Width = maxWidth,
                }
            };
        }
    }
}
