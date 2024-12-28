using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.Enums;
using System.ComponentModel;

namespace Fide.Module.BusinessObjects;

public class AnalysisResult : FideBaseObject
{
    [ReadOnly(true)]
    public virtual ICollection<FileData> Files { get; set; }
    [ReadOnly(true)]
    public virtual string Information { get; set; }
    [ReadOnly(true)]
    public virtual AnalysisEnum AnalysisType { get; set; }
}
