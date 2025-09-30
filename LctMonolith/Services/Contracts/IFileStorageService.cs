namespace LctMonolith.Services.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream content, string contentType, string keyPrefix, CancellationToken ct = default);
    Task DeleteAsync(string key, CancellationToken ct = default);
    Task<string> GetPresignedUrlAsync(string key, TimeSpan? expires = null, CancellationToken ct = default);
}
