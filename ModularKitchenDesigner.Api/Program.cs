using Serilog;
using TelegramService.DependencyInjection;
using HttpConnector.DependencyInjection;
using ModularKitchenDesigner.DAL;
using ModularKitchenDesigner.DAL.Dependencies;
using Repository.Dependencies;
using ModularKitchenDesigner.Api;
using ModularKitchenDesigner.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddHttpConnector();

builder.Services.AddTelegramService();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddRepositoryFactory<ApplicationDbContext>();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1.0");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Api v2.0");
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "Api v3.0");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
