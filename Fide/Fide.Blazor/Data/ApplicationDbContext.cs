using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fide.Blazor.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ImageLink> ImageLinks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator dbCreater)
        {
            if (!dbCreater.CanConnect())
            {
                dbCreater.Create();
            }
            if (!dbCreater.HasTables())
            {
                dbCreater.CreateTables();
            }
        }
    }
}
