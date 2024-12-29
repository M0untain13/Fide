using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.BusinessObjects.Security;
using Fide.Module.Helpers;
using System.Collections.ObjectModel;

namespace Fide.Module.BusinessObjects;

public class ImageAnalysis : FideBaseObject
{
    public virtual FileData Image { get; set; }
    [Aggregated]
    public virtual IList<AnalysisResult> Results { get; set; } = new ObservableCollection<AnalysisResult>();
    public virtual bool IsShared { get; set; }
    public virtual ApplicationUser Owner { get; set; } 

    public override void OnCreated()
    {
        base.OnCreated();

        Owner = CurrentUserHelper.GetCurrentUser();
    }
}
