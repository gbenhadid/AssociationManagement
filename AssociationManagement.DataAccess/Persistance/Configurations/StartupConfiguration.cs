using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AssociationManagement.Tools.Logging;

namespace AssociationManagement.DataAccess.Persistance.Configurations {
    public static class StartupConfiguration {
        public static IHost MigrateDatabase(this IHost host) {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();
            try {
                if(dbContext.Database.GetPendingMigrations().Any()) {
                    dbContext.Database.Migrate();
                }
            } catch(Exception ex) {
                logger.LogError($"An error occured while migrating : {ex.Message}");
            }
            return host;
        }
    }
}
