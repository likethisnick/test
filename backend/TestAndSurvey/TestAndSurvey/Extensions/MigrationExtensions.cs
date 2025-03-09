using Microsoft.EntityFrameworkCore;
using TestAndSurvey.DataAccess;

namespace TestAndSurvey.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
   //     using IServiceScope scope = app.ApplicationServices.CreateScope();
        
   //     using SurvefyDbContext context = scope.ServiceProvider.GetRequiredService<SurvefyDbContext>();
        
   //     context.Database.Migrate();
    }
}