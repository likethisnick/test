using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.Models;

namespace TestAndSurvey.DataAccess;

public class SurvefyDbContext(DbContextOptions<SurvefyDbContext> options) : IdentityDbContext<SurvefyUser>(options)
{
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<TemplateSurvey> TemplateSurvey => Set<TemplateSurvey>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("dbo");
    }
}   