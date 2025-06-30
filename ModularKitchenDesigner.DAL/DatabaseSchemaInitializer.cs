using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace ModularKitchenDesigner.DAL
{
    public class Result
    {
        public int Value { get; set; }
    };

    public static class AddDatabaseSchemaInitializer
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            await CreateMysqlLogsTableAsync(context);
            await CreateEventsAsync(context);
        }

        private static async Task CreateMysqlLogsTableAsync(ApplicationDbContext context)
        {
            // Проверяем существование таблицы через information_schema
            var tableExists = await context.Database.SqlQueryRaw<Result>(
                "SELECT COUNT(*) FROM information_schema.TABLES " +
                "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'error_logs'")
                .ToListAsync();

            if (tableExists[0].Value == 0)
            {
                var sql = ReadEmbeddedResource("CreateErrorLogsTable.sql");
                await context.Database.ExecuteSqlRawAsync(sql);
            };
        }

        private static string ReadEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"ModularKitchenDesigner.DAL.SqlScripts.{fileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static async Task CreateEventsAsync(ApplicationDbContext context)
        {
            var dbName = context.Database.GetDbConnection().Database;

            // Проверка и создание события
            var eventExists = await context.Database.SqlQueryRaw<Result>(
                "SELECT COUNT(*) FROM information_schema.EVENTS " +
                "WHERE EVENT_SCHEMA = @dbName " +
                "AND EVENT_NAME = 'remove_outdated_kitchens_event'",
                new MySqlParameter("@dbName", dbName)).ToListAsync();

            if (eventExists[0].Value == 0)
            {
                var sql = ReadEmbeddedResource("CreateRemoveOutdatedKitchensEvent.sql");
                await context.Database.ExecuteSqlRawAsync(sql);

                await context.Database.ExecuteSqlRawAsync(
                    "ALTER EVENT modular_kitchen_designer.remove_outdated_kitchens_event " +
                    "ON SCHEDULE EVERY 1 HOUR " +
                    $"STARTS '{DateTime.Now.AddHours(6):yyyy-MM-dd HH:mm:ss}';");
            }
        }
    }
}
