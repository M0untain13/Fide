using Microsoft.AspNetCore.Identity;

namespace Fide.Blazor.Data;

public class ApplicationUser : IdentityUser
{
    public virtual List<ImageLink> ImageLinks { get; set; }
}
