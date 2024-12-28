using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.BusinessObjects.Security;
using System.ComponentModel;

namespace Fide.Module.BusinessObjects;

public class ImageAnalysis : FideBaseObject
{
    public virtual FileData Image { get; set; }
    [Aggregated]
    public virtual ICollection<AnalysisResult> Results { get; set; }
    public virtual bool IsShared { get; set; }

    [ReadOnly(true)]
    public virtual ApplicationUser Owner { get; set; }
}
