namespace LctMonolith.Application.Options;

public class S3StorageOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Bucket { get; set; } = "avatars";
    public string? PublicBaseUrl { get; set; } // optional CDN / reverse proxy base
    public int PresignExpirationMinutes { get; set; } = 60;
}
