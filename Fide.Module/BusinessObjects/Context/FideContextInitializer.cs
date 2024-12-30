using DevExpress.ExpressApp.EFCore.DesignTime;
using Microsoft.EntityFrameworkCore;

namespace Fide.Module.BusinessObjects.Context;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891/core-prerequisites-for-design-time-model-editor-with-entity-framework-core-data-model.
public class FideContextInitializer : DbContextTypesInfoInitializerBase
{
    protected override DbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<FideEFCoreDbContext>()
            .UseNpgsql(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new FideEFCoreDbContext(optionsBuilder.Options);
    }
}
