using Microsoft.Extensions.DependencyInjection;
using ModularKitchenDesigner.Application.Converters;
using ModularKitchenDesigner.Application.Handlers;
using ModularKitchenDesigner.Application.Processors;
using ModularKitchenDesigner.Application.Services;
using ModularKitchenDesigner.Application.Validators;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Handlers;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;

namespace ModularKitchenDesigner.Application.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            if (services is null)
            {
                throw new ApplicationException(nameof(services));
            }

            services.AddScoped<IValidatorFactory, ValidatorFactory>();

            services.AddScoped<IExchangeService<NomanclatureDto>, ExchengeWith1СService>();

            services.AddScoped<IProcessorFactory, ProcessorFactory>();

            services.AddScoped<ISimpleEntityProcessorFactory, SimpleEntityProcessorFactory>();

            services.AddScoped<IDtoToEntityConverterFactory, DtoToEntityConverterFactory>();

            services.AddSingleton<IExceptionHandlerService, ExceptionHandlerService>();


            return services;
        }
    }
}
