using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace ModularKitchenDesigner.Api
{
    public static class StartUp
    {
        /// <summary>
        /// Подключение Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            object value = services.AddApiVersioning()
                .AddApiExplorer(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Сервис ModularKitchenDesigner.Api. Методы для обмена.",
                    Description = "Версия 1.0",
                });

                options.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "Сервис ModularKitchenDesigner.Api",
                    Description = "Версия 2.0",
                });

                options.SwaggerDoc("v3", new OpenApiInfo()
                {
                    Version = "v3",
                    Title = "Сервис ModularKitchenDesigner.Api",
                    Description = "Версия 3.0",
                });
                
                options.SwaggerDoc("v4", new OpenApiInfo()
                {
                    Version = "v4",
                    Title = "Сервис ModularKitchenDesigner.Api",
                    Description = "Версия 4.0",
                });

                options.SwaggerDoc("v5", new OpenApiInfo()
                {
                    Version = "v5",
                    Title = "Сервис ModularKitchenDesigner.Api",
                    Description = "Версия 5.0",
                });
            });
        }
    }
}
