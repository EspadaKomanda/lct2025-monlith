using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/ranks")]
[Authorize]
public class RanksController : ControllerBase
{
    private readonly IRankService _rankService;
    private readonly IRuleValidationService _ruleValidation;

    public RanksController(IRankService rankService, IRuleValidationService ruleValidation)
    {
        _rankService = rankService;
        _ruleValidation = ruleValidation;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ranks = await _rankService.GetAllRanksAsync();
        return Ok(ranks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var r = await _rankService.GetRankByIdAsync(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateRankDto dto)
    {
        var rank = await _rankService.CreateRankAsync(new Rank { Title = dto.Title, ExpNeeded = dto.ExpNeeded });
        return CreatedAtAction(nameof(Get), new { id = rank.Id }, rank);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateRankDto dto)
    {
        var r = await _rankService.GetRankByIdAsync(id);
        if (r == null) return NotFound();
        r.Title = dto.Title;
        r.ExpNeeded = dto.ExpNeeded;
        await _rankService.UpdateRankAsync(r);
        return Ok(r);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _rankService.DeleteRankAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("validate-advance/{playerId:guid}/{targetRankId:guid}")]
    public async Task<IActionResult> CanAdvance(Guid playerId, Guid targetRankId)
    {
        var ok = await _ruleValidation.ValidateRankAdvancementRulesAsync(playerId, targetRankId);
        return Ok(new { playerId, targetRankId, canAdvance = ok });
    }
}

