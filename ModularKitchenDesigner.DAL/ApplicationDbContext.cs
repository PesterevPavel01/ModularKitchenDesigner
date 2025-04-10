using Interceptors;
using Microsoft.EntityFrameworkCore;
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
            /*
            modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                Name = "tech",
                Code = "default",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
            */
        }
    }
}
