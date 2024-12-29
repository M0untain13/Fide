using DevExpress.ExpressApp;
using Fide.Module.BusinessObjects.Security;

namespace Fide.Module.Helpers;

public static class CurrentUserHelper
{
    public static ApplicationUser GetCurrentUser()
    {
        return SecuritySystem.CurrentUser as ApplicationUser;
    }
}
