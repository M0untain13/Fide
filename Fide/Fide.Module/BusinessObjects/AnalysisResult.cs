using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.Enums;
using System.Collections.ObjectModel;

namespace Fide.Module.BusinessObjects;

public class AnalysisResult : FideBaseObject
{
    public virtual IList<FileData> Files { get; set; } = new ObservableCollection<FileData>();
    public virtual string Information { get; set; }
    public virtual AnalysisEnum AnalysisType { get; set; }
}
