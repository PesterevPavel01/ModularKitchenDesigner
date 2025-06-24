using HttpConnector.DependencyInjection;
using ModularKitchenDesigner.Api;
using ModularKitchenDesigner.Api.Middlewares;
using ModularKitchenDesigner.Application.Exchange;
using ModularKitchenDesigner.Application.Extensions;
using ModularKitchenDesigner.DAL;
using ModularKitchenDesigner.DAL.Dependencies;
using Repository.Dependencies;
using Serilog;
using TelegramService.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddHttpConnector();
builder.Services.Configure<ExchangeRules>(builder.Configuration.GetSection("ExchangeRules"));
builder.Services.AddTelegramService();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddRepositoryFactory<ApplicationDbContext>();
builder.Services.AddServices();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1.0");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Api v2.0");
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "Api v3.0");
    c.SwaggerEndpoint("/swagger/v4/swagger.json", "Api v4.0");
    c.SwaggerEndpoint("/swagger/v5/swagger.json", "Api v5.0");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
