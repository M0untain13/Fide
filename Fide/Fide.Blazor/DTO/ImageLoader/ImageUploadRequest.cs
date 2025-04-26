namespace Fide.Blazor.DTO.ImageLoader;

public class ImageUploadRequest
{
    public required string FileName { get; set; }
    public required Stream Stream { get; set; }
}
