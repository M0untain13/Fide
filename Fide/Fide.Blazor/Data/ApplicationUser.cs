using Microsoft.AspNetCore.Identity;

namespace Fide.Blazor.Data;

public class ApplicationUser : IdentityUser
{
    public virtual IList<ImageLink> ImageLinks { get; set; } = [];
}
