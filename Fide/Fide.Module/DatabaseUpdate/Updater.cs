using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Fide.Module.BusinessObjects;
using Fide.Module.BusinessObjects.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Fide.Module.DatabaseUpdate;

public class Updater(IObjectSpace objectSpace, Version currentDBVersion) 
    : ModuleUpdater(objectSpace, currentDBVersion)
{
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();

        var adminRole = CreateAdminRole();
        var defaultRole = CreateDefaultRole();

        ObjectSpace.CommitChanges();

        AddAdmin(adminRole);
#if DEBUG
        AddDefaultUserForDebug(defaultRole);
#endif
        ObjectSpace.CommitChanges();
    }

    private void AddAdmin(PermissionPolicyRole adminRole)
    {
        UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

        if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "Admin") == null)
        {
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "Admin", EmptyPassword, (user) =>
            {
                user.Roles.Add(adminRole);
            });
        }
    }

    private void AddDefaultUserForDebug(PermissionPolicyRole defaultRole)
    {
        UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

        if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "User") == null)
        {
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "User", EmptyPassword, (user) =>
            {
                user.Roles.Add(defaultRole);
            });
        }
    }

    private PermissionPolicyRole CreateAdminRole()
    {
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
            adminRole.IsAdministrative = true;
        }
        return adminRole;
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Read, 
                cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), 
                SecurityPermissionState.Allow
            );
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Write, 
                "ChangePasswordOnFirstLogon",
                cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
                SecurityPermissionState.Allow
            );
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Write,
                "StoredPassword",
                cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
                SecurityPermissionState.Allow
            );

            defaultRole.AddNavigationPermission(
                @"Application/NavigationItems/Items/MyAccount",
                SecurityPermissionState.Allow
            );
            defaultRole.AddNavigationPermission(
                @"Application/NavigationItems/Items/MyImages",
                SecurityPermissionState.Allow
            );
            defaultRole.AddNavigationPermission(
                @"Application/NavigationItems/Items/SharedImages",
                SecurityPermissionState.Allow
            );
            
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(
                SecurityOperations.Read, 
                SecurityPermissionState.Deny
            );
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(
                SecurityOperations.Create, 
                SecurityPermissionState.Allow
            );
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(
                SecurityOperations.Create, 
                SecurityPermissionState.Allow
            );
            defaultRole.AddTypePermissionsRecursively<ImageAnalysis>(
                SecurityOperations.CRUDAccess,
                SecurityPermissionState.Allow
            );

            defaultRole.AddObjectPermission<ModelDifference>(
                SecurityOperations.ReadWriteAccess, 
                "UserId = ToStr(CurrentUserId())", 
                SecurityPermissionState.Allow
            );
            defaultRole.AddObjectPermission<ModelDifferenceAspect>(
                SecurityOperations.ReadWriteAccess, 
                "Owner.UserId = ToStr(CurrentUserId())", 
                SecurityPermissionState.Allow
            );
        }
        return defaultRole;
    }
}
