using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/players")]
[Authorize]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IProgressTrackingService _progressService;

    public PlayersController(IPlayerService playerService, IProgressTrackingService progressService)
    {
        _playerService = playerService;
        _progressService = progressService;
    }

    [HttpGet("{playerId:guid}")] 
    public async Task<IActionResult> GetPlayer(Guid playerId)
    {
        var player = await _playerService.GetPlayerWithProgressAsync(playerId);
        return Ok(player);
    }

    [HttpGet("user/{userId:guid}")] 
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        var p = await _playerService.GetPlayerByUserIdAsync(userId.ToString());
        if (p == null) return NotFound();
        return Ok(p);
    }

    public class CreatePlayerRequest { public Guid UserId { get; set; } public string Username { get; set; } = string.Empty; }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreatePlayerRequest req)
    {
        var p = await _playerService.CreatePlayerAsync(req.UserId.ToString(), req.Username);
        return CreatedAtAction(nameof(GetPlayer), new { playerId = p.Id }, p);
    }

    public record AdjustValueRequest(int Value);

    [HttpPost("{playerId:guid}/experience")]
    [Authorize(Roles = "Admin")] // manual adjust
    public async Task<IActionResult> AddExperience(Guid playerId, AdjustValueRequest r)
    {
        var p = await _playerService.AddPlayerExperienceAsync(playerId, r.Value);
        return Ok(new { p.Id, p.Experience });
    }

    [HttpPost("{playerId:guid}/mana")]
    [Authorize(Roles = "Admin")] // manual adjust
    public async Task<IActionResult> AddMana(Guid playerId, AdjustValueRequest r)
    {
        var p = await _playerService.AddPlayerManaAsync(playerId, r.Value);
        return Ok(new { p.Id, p.Mana });
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTop([FromQuery] int count = 10)
    {
        var list = await _playerService.GetTopPlayersAsync(count, TimeSpan.FromDays(30));
        return Ok(list);
    }

    [HttpGet("{playerId:guid}/progress")] 
    public async Task<IActionResult> GetProgress(Guid playerId)
    {
        var prog = await _progressService.GetPlayerOverallProgressAsync(playerId);
        return Ok(prog);
    }
}

