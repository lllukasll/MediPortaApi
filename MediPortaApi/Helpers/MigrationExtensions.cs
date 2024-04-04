using Microsoft.EntityFrameworkCore;

namespace MediPortaApi.Helpers
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if(dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
