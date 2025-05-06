using Fide.Blazor.Data;
using Fide.Blazor.Services.FileStorage;
using Fide.Blazor.Services.Repositories.Base;

namespace Fide.Blazor.Services.ImageManager;

public class ImageManager(IFileStorage storage, IEntityRepository<ImageLink> imageRepository) : IImageManager
{
    public async Task DeleteImageAsync(ImageLink image)
    {
        await storage.DeleteFileAsync(image.Url);
        imageRepository.Delete(image.Id);
        imageRepository.Save();
    }

    public async Task<ImageLink> UploadImageAsync(Stream stream, string fileName)
    {
        var nameInBucket = await storage.UploadFileAsync(stream, fileName);
        var image = new ImageLink()
        {
            Url = storage.GetFileUrl(nameInBucket),
        };
        imageRepository.Create(image);
        imageRepository.Save();
        return image;
    }
}
