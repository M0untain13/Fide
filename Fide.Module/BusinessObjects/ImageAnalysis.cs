using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.BusinessObjects.Security;

namespace Fide.Module.BusinessObjects;

public class ImageAnalysis : FideBaseObject
{
    public FileData Image { get; set; }
    public ICollection<AnalysisResult> Results { get; set; }
    public ApplicationUser Owner { get; set; }
}
