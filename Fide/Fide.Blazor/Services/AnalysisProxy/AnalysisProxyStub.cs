using Fide.Blazor.Models.AnalysisModels;

namespace Fide.Blazor.Services.AnalysisProxy
{
    public class AnalysisProxyStub : IAnalysisProxy
    {
        private readonly string _baseUrl;

        public AnalysisProxyStub(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public AnalysisResponse Send(AnalysisRequest request)
        {
            return new AnalysisResponse();
        }
    }
}
