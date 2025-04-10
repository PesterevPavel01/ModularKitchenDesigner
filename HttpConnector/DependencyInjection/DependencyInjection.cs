using HttpConnector.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HttpConnector.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddHttpConnector(this IServiceCollection services)
        {

            InitServices(services);
        }

        private static void InitServices(this IServiceCollection services)
        {

            services.AddScoped<IHttpConnector, Services.HttpConnector>();

        }
    }
}
