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
            await CreateEventsAsync(context);
            await CreateTriggersAsync(context);
        }

        private static async Task CreateEventsAsync(ApplicationDbContext _context)
        {
            var dbName = _context.Database.GetDbConnection().Database;

            // Проверка и создание события
            var eventExists = await _context.Database.SqlQueryRaw<Result>(
                "SELECT COUNT(*) FROM information_schema.EVENTS " +
                "WHERE EVENT_SCHEMA = @dbName " +
                "AND EVENT_NAME = 'remove_outdated_kitchens_event'",
                new MySqlParameter("@dbName", dbName)).ToListAsync();

            if (eventExists[0].Value == 0)
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    CREATE EVENT remove_outdated_kitchens_event
                    ON SCHEDULE EVERY 1 HOUR
                    STARTS CURRENT_TIMESTAMP
                    DO 
                    BEGIN
                        DELETE FROM kitchens WHERE CreatedAt < NOW() - INTERVAL 1 DAY;
                    END;
                    
                    ALTER EVENT remove_outdated_kitchens_event ENABLE;");
            }
        }

        private static async Task CreateTriggersAsync(ApplicationDbContext _context)
        {
            var dbName = _context.Database.GetDbConnection().Database;

            // Проверка и создание триггера
            var triggerExists = await _context.Database.SqlQueryRaw<Result>(
                "SELECT COUNT(*) FROM information_schema.TRIGGERS " +
                "WHERE TRIGGER_SCHEMA = @dbName " +
                "AND TRIGGER_NAME = 'keep_only_latest_kitchen'",
                new MySqlParameter("@dbName", dbName)).ToListAsync();

            if (triggerExists[0].Value == 0)
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    CREATE TRIGGER keep_only_latest_kitchen
                    BEFORE INSERT ON kitchens
                    FOR EACH ROW
                    BEGIN
                        DELETE FROM material_specification_items
                        WHERE KitchenId IN (
                            SELECT k.Id FROM kitchens k 
                            WHERE k.UserId = NEW.UserId 
                            AND k.Id != NEW.Id
                        );
                        
                        DELETE FROM sections
                        WHERE KitchenId IN (
                            SELECT k.Id FROM kitchens k 
                            WHERE k.UserId = NEW.UserId 
                            AND k.Id != NEW.Id
                        );
                    END;");
            }
        }
    }
}
