using Microsoft.EntityFrameworkCore.Design;

namespace Fide.Module.BusinessObjects.Context;

//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class FideDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FideEFCoreDbContext>
{
    public FideEFCoreDbContext CreateDbContext(string[] args)
    {
        throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        //var optionsBuilder = new DbContextOptionsBuilder<FideEFCoreDbContext>();
        //optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Fide");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
        //return new FideEFCoreDbContext(optionsBuilder.Options);
    }
}
