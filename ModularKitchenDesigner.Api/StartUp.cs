using System.Reflection;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModularKitchenDesigner.Domain.Dto.Authorization;

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
            object value = services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version"),
                    new UrlSegmentApiVersionReader());
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
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

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Введите валидный токен",
                    Name = "Авторизация",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            Array.Empty<string>()
                        }
                    }
                );

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }

        /// <summary>
        /// Настройка авторизацмм
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutorization(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("Authorization").Get<AuthorizationSetts>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy =>
                    policy.RequireRole("Administrator"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                byte[] secretBytes = Encoding.UTF8.GetBytes(authSettings.SecretKey);

                var key = new SymmetricSecurityKey(secretBytes);

                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authSettings.Issuer,
                    ValidAudience = authSettings.Audience,
                    IssuerSigningKey = key,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                config.Authority = authSettings.Authority;
                config.RequireHttpsMetadata = false;
            });
        }
    }
}
