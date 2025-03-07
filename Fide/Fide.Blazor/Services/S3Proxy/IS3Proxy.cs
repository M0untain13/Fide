using Fide.Blazor.Models.S3Models;

namespace Fide.Blazor.Services.S3Proxy;

public interface IS3Proxy
{
    S3Response Send(S3Request request);
}
