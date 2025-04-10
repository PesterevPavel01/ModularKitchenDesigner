using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.Dependencies
{
    public static class Dependencies
    {
        public static void AddRepositoryFactory<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, RepositiryFactory<TContext>>();
        }
    }
}
