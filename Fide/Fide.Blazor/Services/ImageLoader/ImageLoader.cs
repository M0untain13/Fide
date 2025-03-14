using Fide.Blazor.Data;
using Fide.Blazor.Models.ImageLoaderModels;
using Fide.Blazor.Models.S3Models;
using Fide.Blazor.Services.Repositories.Base;
using Fide.Blazor.Services.S3Proxy;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fide.Blazor.Services.ImageLoader;

public class ImageLoader(
    IS3Proxy s3proxy, 
    IRepository<ImageLink> imageRepository, 
    IRepository<ApplicationUser, string> userRepository,
    AuthenticationState context
) : IImageLoader
{
    public ImageDeleteResponse Delete(ImageDeleteRequest request)
    {
        // Проверить существование изображения
        // Удалить из S3
        // Удалить ссылку из БД
    }

    public ImageDownloadResponse Download(ImageDownloadRequest request)
    {
        var imageLinks = imageRepository
            .GetAll()
            .Where(l => l.User.UserName == request.UserName)
            .Skip(request.Skip)
            .Take(request.Take);

        var fileStreams = new List<Stream>();

        foreach (var imageLink in imageLinks)
        {
            var s3request = new S3GetRequest()
            {
                ObjectName = imageLink.Url,
            };
            var s3response = s3proxy.GetAsync(s3request).Result;
            fileStreams.Add(s3response.Stream);
        }

        var response = new ImageDownloadResponse()
        {
            FileStreams = fileStreams,
        };
        return response;
    }

    public ImageUploadResponse Upload(ImageUploadRequest request)
    {
        var imageUrl = new List<string>();
        foreach(var stream in request.FileStreams)
        {
            var s3request = new S3UploadRequest()
            {
                Stream = stream,
            };
            var s3response = s3proxy.UploadAsync(s3request).Result;
            var user = userRepository.Get(context.User.Identity.Name);
            var imageLink = new ImageLink()
            {
                User = user,
                Url = s3response.ObjectName,
            };
            imageRepository.Create(imageLink);
            imageUrl.Add(imageLink.Url);
        }
        var response = new ImageUploadResponse()
        {
            ImageUrls = imageUrl,
        };
        return response;
    }
}
