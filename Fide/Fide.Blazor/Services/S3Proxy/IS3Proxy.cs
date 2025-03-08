using Fide.Blazor.Models.S3Models;

namespace Fide.Blazor.Services.S3Proxy;

public interface IS3Proxy
{
    Task<S3UploadResponse> UploadAsync(S3UploadRequest request);
    Task<S3GetResponse> GetAsync(S3GetRequest request);
}
