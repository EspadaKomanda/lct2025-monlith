using LctMonolith.Models;

namespace LctMonolith.Services.Contracts;

public interface INotificationService
{
    Task<Notification> CreateAsync(Guid userId, string type, string title, string message, CancellationToken ct = default);
    Task<IEnumerable<Notification>> GetUnreadAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<Notification>> GetAllAsync(Guid userId, int take = 100, CancellationToken ct = default);
    Task MarkReadAsync(Guid userId, Guid notificationId, CancellationToken ct = default);
    Task<int> MarkAllReadAsync(Guid userId, CancellationToken ct = default);
}
