using Interceptors;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using System.Reflection;

namespace ModularKitchenDesigner.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.AddInterceptors(new DateInterceptors());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            
            modelBuilder.Entity<PriceSegment>().HasData(
                PriceSegment.Create<PriceSegment>(title: "default", code: "00000000DEF", id: Guid.NewGuid()));

            modelBuilder.Entity<Material>().HasData(
                PriceSegment.Create<Material>(title: "default", code: "00000000DEF", id: Guid.NewGuid()));
        }
    }
}
