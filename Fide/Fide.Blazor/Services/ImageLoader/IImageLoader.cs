using Fide.Blazor.Models.ImageLoaderModels;

namespace Fide.Blazor.Services.ImageLoader;

public interface IImageLoader
{
    ImageDownloadResponse Download(ImageDownloadRequest request);
    ImageUploadResponse Upload(ImageUploadRequest request);
}