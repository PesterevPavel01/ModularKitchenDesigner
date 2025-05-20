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
            new PriceSegment
            {
                Id = Guid.NewGuid(),
                Title = "default",
                Code = "00000000DEF",
                CreatedAt = DateTime.Now,
                UpdatedAt = default,
            });           
        }
    }
}
