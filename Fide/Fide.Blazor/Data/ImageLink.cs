using Fide.Blazor.Data.Base;

namespace Fide.Blazor.Data;

public class ImageLink : Entity
{
    public virtual required ApplicationUser User { get; set; }
    public virtual string Url { get; set; } = string.Empty;
}
