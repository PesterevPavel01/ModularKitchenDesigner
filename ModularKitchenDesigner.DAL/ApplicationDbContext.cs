using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Authorization;

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

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes("Qwerty123!"));

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser(Guid.NewGuid())
                {
                    UserName = "Administrator",
                    Password = BitConverter.ToString(bytes).ToLower(),
                });
        }
    }
}
