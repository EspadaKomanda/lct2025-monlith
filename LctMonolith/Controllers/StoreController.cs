using System.Security.Claims;
using LctMonolith.Services;
using LctMonolith.Services.Contracts;
using LctMonolith.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/store")]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetItems(CancellationToken ct)
    {
        var items = await _storeService.GetActiveItemsAsync(ct);
        return Ok(items);
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase(PurchaseRequest req, CancellationToken ct)
    {
        var inv = await _storeService.PurchaseAsync(GetUserId(), req.ItemId, req.Quantity, ct);
        return Ok(new { inv.StoreItemId, inv.Quantity, inv.AcquiredAt });
    }
}
