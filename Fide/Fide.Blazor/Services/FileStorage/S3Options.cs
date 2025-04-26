namespace Fide.Blazor.Services.FileStorage;

public class S3Options
{
    public string ServiceURL { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = "default";
    public bool ForcePathStyle { get; set; } = true;
}
