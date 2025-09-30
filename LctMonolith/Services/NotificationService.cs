using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

/// <summary>
/// In-app user notifications CRUD / read-state operations.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _uow;

    public NotificationService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Notification> CreateAsync(Guid userId, string type, string title, string message, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Required", nameof(type));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Required", nameof(title));
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Required", nameof(message));

            var n = new Notification
            {
                UserId = userId,
                Type = type.Trim(),
                Title = title.Trim(),
                Message = message.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _uow.Notifications.AddAsync(n, ct);
            await _uow.SaveChangesAsync(ct);
            return n;
        }
        catch (Exception ex) { Log.Error(ex, "Notification CreateAsync failed {UserId}", userId); throw; }
    }

    public async Task<IEnumerable<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct = default)
    {
        try { return await _uow.Notifications.Query(n => n.UserId == userId && !n.IsRead, q => q.OrderByDescending(x => x.CreatedAt)).Take(100).ToListAsync(ct); }
        catch (Exception ex) { Log.Error(ex, "GetUnreadAsync failed {UserId}", userId); throw; }
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(Guid userId, int take = 100, CancellationToken ct = default)
    {
        try { if (take <= 0) take = 1; if (take > 500) take = 500; return await _uow.Notifications.Query(n => n.UserId == userId, q => q.OrderByDescending(x => x.CreatedAt)).Take(take).ToListAsync(ct); }
        catch (Exception ex) { Log.Error(ex, "GetAllAsync failed {UserId}", userId); throw; }
    }

    public async Task MarkReadAsync(Guid userId, Guid notificationId, CancellationToken ct = default)
    {
        try { var notif = await _uow.Notifications.Query(n => n.Id == notificationId && n.UserId == userId).FirstOrDefaultAsync(ct) ?? throw new KeyNotFoundException("Notification not found"); if (!notif.IsRead) { notif.IsRead = true; notif.ReadAt = DateTime.UtcNow; await _uow.SaveChangesAsync(ct); } }
        catch (Exception ex) { Log.Error(ex, "MarkReadAsync failed {NotificationId}", notificationId); throw; }
    }

    public async Task<int> MarkAllReadAsync(Guid userId, CancellationToken ct = default)
    {
        try { var unread = await _uow.Notifications.Query(n => n.UserId == userId && !n.IsRead).ToListAsync(ct); if (unread.Count == 0) return 0; var now = DateTime.UtcNow; foreach (var n in unread) { n.IsRead = true; n.ReadAt = now; } await _uow.SaveChangesAsync(ct); return unread.Count; }
        catch (Exception ex) { Log.Error(ex, "MarkAllReadAsync failed {UserId}", userId); throw; }
    }
}
