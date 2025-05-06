using System;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;

namespace Fide.Blazor.Services.FileStorage;

public class S3Service(IAmazonS3 s3Client, IOptions<S3Options> options) : IFileStorage
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly S3Options _options = options.Value;

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _options.BucketName);
        if (!bucketExists)
            throw new InvalidOperationException($"Bucket {_options.BucketName} не существует");

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName);
        string newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";

        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = newFileName,
            InputStream = fileStream
        };

        await _s3Client.PutObjectAsync(request);
        return request.Key;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        await _s3Client.DeleteObjectAsync(_options.BucketName, fileName);
    }

    public string GetFileUrl(string fileName)
    {
        return $"{_options.ServiceURL}/{_options.BucketName}/{fileName}";
    }

    public async Task<string> GeneratePresignedUrl(string fileName, TimeSpan expiry)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.Add(expiry)
        };

        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }
}