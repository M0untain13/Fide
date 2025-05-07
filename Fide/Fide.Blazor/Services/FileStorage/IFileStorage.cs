namespace Fide.Blazor.Services.FileStorage;

public interface IFileStorage
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string filePath);
    Task<string> GeneratePresignedUrl(string fileName, TimeSpan expiry);
}