namespace LctMonolith.Models.Database;

public class Profile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? About { get; set; }
    public string? Location { get; set; }

    // Avatar in S3 / MinIO
    public string? AvatarS3Key { get; set; }
    public string? AvatarUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
