using Fide.Blazor.Models.S3Models;

namespace Fide.Blazor.Services.S3Proxy;

public class S3ProxyStub : IS3Proxy
{
    public Task<S3GetResponse> GetAsync(S3GetRequest request)
    {
        return Task.Run(() =>
        {
            return new S3GetResponse();
        });
    }

    public Task<S3UploadResponse> UploadAsync(S3UploadRequest request)
    {
        return Task.Run(() =>
        {
            return new S3UploadResponse();
        });
    }
}
