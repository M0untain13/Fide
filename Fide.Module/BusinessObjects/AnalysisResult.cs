using DevExpress.Persistent.BaseImpl.EF;
using Fide.Module.BusinessObjects.Base;
using Fide.Module.Enums;

namespace Fide.Module.BusinessObjects;

public class AnalysisResult : FideBaseObject
{
    public ICollection<FileData> Files { get; set; }
    public string Information { get; set; }
    public AnalysisEnum AnalysisType { get; set; }
}
