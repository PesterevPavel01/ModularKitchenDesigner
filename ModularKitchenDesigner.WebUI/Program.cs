using ModularKitchenDesigner.WebUI.Components;
using Serilog;
using TelegramService.DependencyInjection;
using HttpConnector.DependencyInjection;
using ModularKitchenDesigner.DAL;
using ModularKitchenDesigner.DAL.Dependencies;
using Repository.Dependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddHttpConnector();

builder.Services.AddTelegramService();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddRepositoryFactory<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
