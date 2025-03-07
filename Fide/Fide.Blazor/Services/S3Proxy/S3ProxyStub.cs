using Fide.Blazor.Models.S3Models;

namespace Fide.Blazor.Services.S3Proxy
{
    public class S3ProxyStub : IS3Proxy
    {
        public S3Response Send(S3Request request)
        {
            return new S3Response();
        }
    }
}
