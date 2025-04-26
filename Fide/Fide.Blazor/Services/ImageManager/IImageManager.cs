using Fide.Blazor.Data;

namespace Fide.Blazor.Services.ImageManager;

public interface IImageManager
{
    public Task DeleteImageAsync(ImageLink image);
    public Task<ImageLink> UploadImageAsync(Stream stream, string fileName);
}
