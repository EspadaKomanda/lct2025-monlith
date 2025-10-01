using System.Security.Claims;
using LctMonolith.Models.Database;
using LctMonolith.Services;
using LctMonolith.Services.Contracts;
using LctMonolith.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RefreshRequest = LctMonolith.Services.Models.RefreshRequest;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenPair>> Register(AuthRequest req, CancellationToken ct)
    {
        var existing = await _userManager.FindByEmailAsync(req.Email);
        if (existing != null)
        {
            return Conflict("Email already registered");
        }
        var user = new AppUser { UserName = req.Email, Email = req.Email, FirstName = req.FirstName, LastName = req.LastName };
        var result = await _userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        var tokens = await _tokenService.IssueAsync(user, ct);
        return Ok(tokens);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenPair>> Login(AuthRequest req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user == null)
        {
            return Unauthorized();
        }
        var passOk = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
        if (!passOk.Succeeded)
        {
            return Unauthorized();
        }
        var tokens = await _tokenService.IssueAsync(user, ct);
        return Ok(tokens);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenPair>> Refresh(RefreshRequest req, CancellationToken ct)
    {
        var pair = await _tokenService.RefreshAsync(req.RefreshToken, ct);
        return Ok(pair);
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke(RevokeRequest req, CancellationToken ct)
    {
        await _tokenService.RevokeAsync(req.RefreshToken, ct);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<object> Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
        return Ok(new { userId = id });
    }
}
