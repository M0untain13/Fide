using DevExpress.ExpressApp;
using Fide.Module.BusinessObjects.Security;

namespace Fide.Module.Helpers;

public static class CurrentUserHelper
{
    public static ApplicationUser GetCurrentUser(IObjectSpace objectSpace)
    {
        var users = objectSpace.GetObjectsQuery<ApplicationUser>();
        var currentUser = users.First(u => u.ID == (SecuritySystem.CurrentUser as ApplicationUser).ID);
        return currentUser;
    }
}
