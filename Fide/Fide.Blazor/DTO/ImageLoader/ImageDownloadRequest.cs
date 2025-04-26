namespace Fide.Blazor.DTO.ImageLoader;

public class ImageDownloadRequest
{
    public required string UserName { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
}
