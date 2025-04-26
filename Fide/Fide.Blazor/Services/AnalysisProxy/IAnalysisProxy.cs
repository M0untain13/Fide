using Fide.Blazor.DTO.Analysis;

namespace Fide.Blazor.Services.AnalysisProxy;

public interface IAnalysisProxy
{
    Task<AnalysisResponse> SendAsync(AnalysisRequest request);
}
