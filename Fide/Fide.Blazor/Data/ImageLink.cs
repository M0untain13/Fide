using Fide.Blazor.Data.Base;

namespace Fide.Blazor.Data;

public class ImageLink : Entity
{
    public virtual ApplicationUser? User { get; set; }
    public virtual string OriginalName { get; set; } = string.Empty;
    public virtual string? AnalysisName { get; set; }
}
