using Microsoft.Extensions.DependencyInjection;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Processors;
using ModularKitchenDesigner.Application.Validators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;
using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IValidatorFactory, ValidatorFactory>();

            services.AddScoped<IProcessorFactory<ComponentType, SimpleDto>, ProcessorFactory<ComponentType, SimpleDto>>();
            services.AddScoped<IProcessorFactory<Material, SimpleDto>, ProcessorFactory<Material, SimpleDto>>();
            services.AddScoped<IProcessorFactory<Model, SimpleDto>, ProcessorFactory<Model, SimpleDto>>();
            services.AddScoped<IProcessorFactory<ModuleType, SimpleDto>, ProcessorFactory<ModuleType, SimpleDto>>();
            services.AddScoped<IProcessorFactory<PriceSegment, SimpleDto>, ProcessorFactory<PriceSegment, SimpleDto>>();


            services.AddScoped<IProcessorFactory<ModelItem, ModelItemDto>, ProcessorFactory<ModelItem, ModelItemDto>>();
            services.AddScoped<IProcessorFactory<Section, OrderDto>, ProcessorFactory<Section, OrderDto>>();
            services.AddScoped<IProcessorFactory<Section, SectionDto>, ProcessorFactory<Section, SectionDto>>();
            services.AddScoped<IProcessorFactory<Module, ModuleDto>, ProcessorFactory<Module, ModuleDto>>();
            services.AddScoped<IProcessorFactory<Kitchen, KitchenDto>, ProcessorFactory<Kitchen, KitchenDto>>();
            services.AddScoped<IProcessorFactory<KitchenType,KitchenTypeDto>, ProcessorFactory<KitchenType, KitchenTypeDto>>();
            services.AddScoped<IProcessorFactory<Component, ComponentDto>, ProcessorFactory<Component, ComponentDto>>();
            services.AddScoped<IProcessorFactory<MaterialSelectionItem,MaterialSelectionItemDto>, ProcessorFactory<MaterialSelectionItem, MaterialSelectionItemDto>>();
            services.AddScoped<IProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto>, ProcessorFactory<MaterialSpecificationItem, MaterialSpecificationItemDto>>();

            services.AddScoped<ISimpleEntityProcessorFactory, SimpleEntityProcessorFactory>();

            services.AddScoped<IDtoToEntityConverterFactory, DtoToEntityConverterFactory>();


            return services;
        }
    }
}
