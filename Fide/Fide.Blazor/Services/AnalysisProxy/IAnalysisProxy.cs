using Fide.Blazor.Models.AnalysisModels;

namespace Fide.Blazor.Services.AnalysisProxy;

public interface IAnalysisProxy
{
    AnalysisResponse Send(AnalysisRequest request);
}
