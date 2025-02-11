using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Fide.Module.BusinessObjects.Security;
using Microsoft.EntityFrameworkCore;

namespace Fide.Module.BusinessObjects.Context;

[TypesInfoInitializer(typeof(FideContextInitializer))]
public class FideEFCoreDbContext : DbContext
{
    public FideEFCoreDbContext(DbContextOptions<FideEFCoreDbContext> options) : base(options) { }
    public DbSet<ModuleInfo> ModulesInfo { get; set; }
    public DbSet<ModelDifference> ModelDifferences { get; set; }
    public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
    public DbSet<PermissionPolicyRole> Roles { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserLoginInfo> UserLoginInfos { get; set; }
    public DbSet<FileData> FileData { get; set; }
    public DbSet<ImageAnalysis> ImageAnalysis { get; set; }
    public DbSet<AnalysisResult> AnalysisResult { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseDeferredDeletion(this);
        modelBuilder.SetOneToManyAssociationDeleteBehavior(DeleteBehavior.SetNull, DeleteBehavior.Cascade);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        modelBuilder.Entity<ApplicationUserLoginInfo>(b =>
        {
            b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
        });
        modelBuilder.Entity<ModelDifference>()
            .HasMany(t => t.Aspects)
            .WithOne(t => t.Owner)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
