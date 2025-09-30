using System.Security.Claims;
using LctMonolith.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    public InventoryController(IInventoryService inventoryService) => _inventoryService = inventoryService;

    private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get inventory for current authenticated user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var items = await _inventoryService.GetStoreInventoryAsync(CurrentUserId(), ct);
        return Ok(items.Select(i => new { i.StoreItemId, i.Quantity, i.AcquiredAt }));
    }

    /// <summary>Admin: get inventory for specific user.</summary>
    [HttpGet("user/{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
    {
        var items = await _inventoryService.GetStoreInventoryAsync(userId, ct);
        return Ok(items);
    }
}
