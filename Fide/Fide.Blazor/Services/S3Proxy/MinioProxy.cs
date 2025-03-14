using Fide.Blazor.Models.S3Models;
using Minio;
using Minio.DataModel.Args;

namespace Fide.Blazor.Services.S3Proxy;

public class MinioProxy(IMinioClient minioClient, ILogger<MinioProxy> logger, string bucketName) : IS3Proxy
{
    private async Task<bool> IsBucketExist()
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
        return await minioClient.BucketExistsAsync(bucketExistsArgs).ConfigureAwait(false);
    }

    private async Task CreateBucket()
    {
        var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
        await minioClient.MakeBucketAsync(makeBucketArgs).ConfigureAwait(false);
    }

    public async Task<S3UploadResponse> UploadAsync(S3UploadRequest request)
    {
        try
        {
            if (!await IsBucketExist().ConfigureAwait(false))
            {
                await CreateBucket().ConfigureAwait(false);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(request.ObjectName)
                .WithStreamData(request.Stream);

            await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            return new S3UploadResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(new EventId(), ex, "Ошибка при загрузке изображения на S3 хранилище");
            throw;
        }
    }

    public async Task<S3GetResponse> GetAsync(S3GetRequest request)
    {
        var response = new S3GetResponse();

        var getArgs = new GetObjectArgs()
            .WithObject(request.ObjectName)
            .WithBucket(bucketName)
            .WithCallbackStream(stream => response.Stream = stream);

        var _ = await minioClient.GetObjectAsync(getArgs);

        return response;
    }
}
