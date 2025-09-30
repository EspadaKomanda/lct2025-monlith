using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using Microsoft.Extensions.Options;
using LctMonolith.Application.Options;
using LctMonolith.Services.Interfaces;
using Serilog;

namespace LctMonolith.Services;

public class S3FileStorageService : IFileStorageService, IDisposable
{
    private readonly S3StorageOptions _opts;
    private readonly IAmazonS3 _client;
    private bool _bucketChecked;

    public S3FileStorageService(IOptions<S3StorageOptions> options)
    {
        _opts = options.Value;
        var cfg = new AmazonS3Config
        {
            ServiceURL = _opts.Endpoint,
            ForcePathStyle = true,
            UseHttp = !_opts.UseSsl,
            Timeout = TimeSpan.FromSeconds(30),
            MaxErrorRetry = 2,
        };
        _client = new AmazonS3Client(_opts.AccessKey, _opts.SecretKey, cfg);
    }

    private async Task EnsureBucketAsync(CancellationToken ct)
    {
        if (_bucketChecked) return;
        try
        {
            var list = await _client.ListBucketsAsync(ct);
            if (!list.Buckets.Any(b => string.Equals(b.BucketName, _opts.Bucket, StringComparison.OrdinalIgnoreCase)))
            {
                await _client.PutBucketAsync(new PutBucketRequest { BucketName = _opts.Bucket }, ct);
            }
            _bucketChecked = true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed ensuring bucket {Bucket}", _opts.Bucket);
            throw;
        }
    }

    public async Task<string> UploadAsync(Stream content, string contentType, string keyPrefix, CancellationToken ct = default)
    {
        await EnsureBucketAsync(ct);
        var key = $"{keyPrefix.Trim('/')}/{DateTime.UtcNow:yyyyMMdd}/{Guid.NewGuid():N}";
        var putReq = new PutObjectRequest
        {
            BucketName = _opts.Bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType
        };
        await _client.PutObjectAsync(putReq, ct);
        Log.Information("Uploaded object {Key} to bucket {Bucket}", key, _opts.Bucket);
        return key;
    }

    public async Task DeleteAsync(string key, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        try
        {
            await _client.DeleteObjectAsync(_opts.Bucket, key, ct);
            Log.Information("Deleted object {Key}", key);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // ignore
        }
    }

    public Task<string> GetPresignedUrlAsync(string key, TimeSpan? expires = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        if (!string.IsNullOrWhiteSpace(_opts.PublicBaseUrl))
        {
            var url = _opts.PublicBaseUrl!.TrimEnd('/') + "/" + key;
            return Task.FromResult(url);
        }
        var req = new GetPreSignedUrlRequest
        {
            BucketName = _opts.Bucket,
            Key = key,
            Expires = DateTime.UtcNow.Add(expires ?? TimeSpan.FromMinutes(_opts.PresignExpirationMinutes))
        };
        var urlSigned = _client.GetPreSignedURL(req);
        return Task.FromResult(urlSigned);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
