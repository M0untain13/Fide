namespace Fide.Blazor.Services.FileStorage;

public interface IFileStorage
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string filePath);
    string GetFileUrl(string filePath);
}