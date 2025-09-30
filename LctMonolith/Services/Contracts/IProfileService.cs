namespace LctMonolith.Services.Interfaces;

using LctMonolith.Models.Database;

public interface IProfileService
{
    Task<Profile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Profile> UpsertAsync(Guid userId, string? firstName, string? lastName, DateOnly? birthDate, string? about, string? location, CancellationToken ct = default);
    Task<Profile> UpdateAvatarAsync(Guid userId, Stream fileStream, string contentType, string? fileName, CancellationToken ct = default);
    Task<bool> DeleteAvatarAsync(Guid userId, CancellationToken ct = default);
}
