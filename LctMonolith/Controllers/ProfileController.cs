using System.Security.Claims;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profiles;
    public ProfileController(IProfileService profiles) => _profiles = profiles;

    private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public class UpdateProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? About { get; set; }
        public string? Location { get; set; }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var p = await _profiles.GetByUserIdAsync(CurrentUserId(), ct);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
    {
        var p = await _profiles.GetByUserIdAsync(userId, ct);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(UpdateProfileDto dto, CancellationToken ct)
    {
        var p = await _profiles.UpsertAsync(CurrentUserId(), dto.FirstName, dto.LastName, dto.BirthDate, dto.About, dto.Location, ct);
        return Ok(p);
    }

    [HttpPost("avatar")]
    [RequestSizeLimit(7_000_000)] // ~7MB
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0) return BadRequest("File required");
        await using var stream = file.OpenReadStream();
        var p = await _profiles.UpdateAvatarAsync(CurrentUserId(), stream, file.ContentType, file.FileName, ct);
        return Ok(new { p.AvatarUrl });
    }

    [HttpDelete("avatar")]
    public async Task<IActionResult> DeleteAvatar(CancellationToken ct)
    {
        var ok = await _profiles.DeleteAvatarAsync(CurrentUserId(), ct);
        return ok ? NoContent() : NotFound();
    }
}
