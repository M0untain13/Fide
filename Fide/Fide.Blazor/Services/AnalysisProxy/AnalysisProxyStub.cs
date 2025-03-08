using Fide.Blazor.Models.AnalysisModels;

namespace Fide.Blazor.Services.AnalysisProxy;

public class AnalysisProxyStub : IAnalysisProxy
{
    public Task<AnalysisResponse> SendAsync(AnalysisRequest request)
    {
        return Task.Run(() =>
        {
            return new AnalysisResponse();
        });
    }
}
