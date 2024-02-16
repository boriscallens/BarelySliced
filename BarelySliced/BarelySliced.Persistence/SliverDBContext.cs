using Microsoft.EntityFrameworkCore;

namespace BarelySliced.Persistence;

// Migration Commands
// dotnet ef migrations add "yourMigrationNameHere" --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
// dotnet ef migrations remove --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
public class SliverDBContext: DbContext
{
    public SliverDBContext(DbContextOptions<SliverDBContext> options):base(options)
    {
    }
}