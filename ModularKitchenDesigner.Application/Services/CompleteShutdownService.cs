using ModularKitchenDesigner.Application.Processors.CommonProcessors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Processors;

namespace ModularKitchenDesigner.Application.Services
{
    public class CompleteShutdownService
    {
        private readonly IProcessorFactory _processorFactory;

        public CompleteShutdownService(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public async Task<String> ShutdownAll()
        {
            await ShutdownEntity<Component, ComponentDto>();
            await ShutdownEntity<ComponentType, SimpleDto>();
            await ShutdownEntity<KitchenType, KitchenTypeDto>();
            await ShutdownEntity<Material, SimpleDto>();
            await ShutdownEntity<MaterialSelectionItem, MaterialSelectionItemDto>();
            await ShutdownEntity<Model, ModelDto>();
            await ShutdownEntity<ModelItem, ModelItemDto>();
            await ShutdownEntity<Module, ModuleDto>();
            await ShutdownEntity<ModuleType, SimpleDto>();
            await ShutdownEntity<PriceSegment, SimpleDto>();

            return "Shutdown all entities";
        }

        private async Task ShutdownEntity<TEntity, TDto>()
            where TDto : BaseDto, IExcangeDtoConvertable<TDto, NomanclatureDto>, IUniqueKeyQueryable<TDto>, new()
            where TEntity : BaseEntity, IDtoConvertible<TEntity, TDto>
        {
            var models = await _processorFactory
                .GetLoaderProcessor<CommonDefaultLoaderProcessor<TEntity, TDto>, TEntity, TDto>()
                .ProcessAsync();

            if (models.Count == 0)
                throw new ArgumentException($"Models {typeof(TEntity).Name} not found");

            await _processorFactory
                    .GetCreatorProcessor<CommonMultipleDisablerProcessor<TEntity, TDto>, TEntity, TDto>()
                    .ProcessAsync([.. models.Data]);
        }
    }
}
