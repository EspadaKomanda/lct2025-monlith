using System.Security.Claims;
using LctMonolith.Services;
using LctMonolith.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notifications;

    public NotificationsController(INotificationService notifications)
    {
        _notifications = notifications;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread(CancellationToken ct)
    {
        var list = await _notifications.GetUnreadAsync(GetUserId(), ct);
        return Ok(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int take = 100, CancellationToken ct = default)
    {
        var list = await _notifications.GetAllAsync(GetUserId(), take, ct);
        return Ok(list);
    }

    [HttpPost("mark/{id:guid}")]
    public async Task<IActionResult> MarkRead(Guid id, CancellationToken ct)
    {
        await _notifications.MarkReadAsync(GetUserId(), id, ct);
        return NoContent();
    }

    [HttpPost("mark-all")]
    public async Task<IActionResult> MarkAll(CancellationToken ct)
    {
        var updated = await _notifications.MarkAllReadAsync(GetUserId(), ct);
        return Ok(new { updated });
    }
}
