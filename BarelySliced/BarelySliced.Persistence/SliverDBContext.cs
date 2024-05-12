using Microsoft.EntityFrameworkCore;

namespace BarelySliced.Persistence;

// Migration Commands
// dotnet ef migrations add "yourMigrationNameHere" --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
// dotnet ef migrations remove --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
public class SliverDbContext: DbContext
{
    public SliverDbContext(DbContextOptions<SliverDbContext> options):base(options)
    {
    }
}