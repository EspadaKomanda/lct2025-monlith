using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LctMonolith.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _storage;

    public ProfileService(IUnitOfWork uow, IFileStorageService storage)
    {
        _uow = uow;
        _storage = storage;
    }

    public async Task<Profile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            return await _uow.Profiles.Query(p => p.UserId == userId).FirstOrDefaultAsync(ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Profile get failed {UserId}", userId);
            throw;
        }
    }

    public async Task<Profile> UpsertAsync(Guid userId, string? firstName, string? lastName, DateOnly? birthDate, string? about, string? location, CancellationToken ct = default)
    {
        try
        {
            var profile = await _uow.Profiles.Query(p => p.UserId == userId).FirstOrDefaultAsync(ct);
            if (profile == null)
            {
                profile = new Profile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    About = about,
                    Location = location
                };
                await _uow.Profiles.AddAsync(profile, ct);
            }
            else
            {
                profile.FirstName = firstName;
                profile.LastName = lastName;
                profile.BirthDate = birthDate;
                profile.About = about;
                profile.Location = location;
                profile.UpdatedAt = DateTime.UtcNow;
                _uow.Profiles.Update(profile);
            }
            await _uow.SaveChangesAsync(ct);
            return profile;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Profile upsert failed {UserId}", userId);
            throw;
        }
    }

    public async Task<Profile> UpdateAvatarAsync(Guid userId, Stream fileStream, string contentType, string? fileName, CancellationToken ct = default)
    {
        try
        {
            var profile = await _uow.Profiles.Query(p => p.UserId == userId).FirstOrDefaultAsync(ct) ??
                           await UpsertAsync(userId, null, null, null, null, null, ct);

            // Delete old if exists
            if (!string.IsNullOrWhiteSpace(profile.AvatarS3Key))
            {
                await _storage.DeleteAsync(profile.AvatarS3Key!, ct);
            }
            var key = await _storage.UploadAsync(fileStream, contentType, $"avatars/{userId}", ct);
            var url = await _storage.GetPresignedUrlAsync(key, TimeSpan.FromHours(6), ct);
            profile.AvatarS3Key = key;
            profile.AvatarUrl = url;
            profile.UpdatedAt = DateTime.UtcNow;
            _uow.Profiles.Update(profile);
            await _uow.SaveChangesAsync(ct);
            return profile;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Avatar update failed {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> DeleteAvatarAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            var profile = await _uow.Profiles.Query(p => p.UserId == userId).FirstOrDefaultAsync(ct);
            if (profile == null || string.IsNullOrWhiteSpace(profile.AvatarS3Key)) return false;
            await _storage.DeleteAsync(profile.AvatarS3Key!, ct);
            profile.AvatarS3Key = null;
            profile.AvatarUrl = null;
            profile.UpdatedAt = DateTime.UtcNow;
            _uow.Profiles.Update(profile);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Delete avatar failed {UserId}", userId);
            throw;
        }
    }
}
