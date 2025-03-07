using Fide.Blazor.Models.AnalysisModels;

namespace Fide.Blazor.Services.AnalysisProxy
{
    public class AnalysisProxyStub : IAnalysisProxy
    {
        public AnalysisResponse Send(AnalysisRequest request)
        {
            return new AnalysisResponse();
        }
    }
}
