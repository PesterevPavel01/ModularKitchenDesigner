using Microsoft.Extensions.DependencyInjection;
using ModularKitchenDesigner.Application.Services.Processors;
using ModularKitchenDesigner.Application.Services.SimpleEntity;
using ModularKitchenDesigner.Application.Validators;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;
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

            services.AddScoped<IProcessorFactory<Module, ModuleDto>, ProcessorFactory<Module, ModuleDto>>();
            services.AddScoped<IProcessorFactory<Block, BlockDto>, ProcessorFactory<Block, BlockDto>>();
            services.AddScoped<IProcessorFactory<Kitchen, KitchenDto>, ProcessorFactory<Kitchen, KitchenDto>>();
            services.AddScoped<IProcessorFactory<KitchenType,KitchenTypeDto>, ProcessorFactory<KitchenType, KitchenTypeDto>>();
            services.AddScoped<IProcessorFactory<Component, ComponentDto>, ProcessorFactory<Component, ComponentDto>>();
            services.AddScoped<IProcessorFactory<MaterialItem,MaterialItemDto>, ProcessorFactory<MaterialItem, MaterialItemDto>>();

            services.AddScoped<ISimpleEntityService, SimpleEntityService>();


            return services;
        }
    }
}
