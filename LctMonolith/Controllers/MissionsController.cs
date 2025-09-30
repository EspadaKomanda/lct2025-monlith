using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/missions")]
[Authorize]
public class MissionsController : ControllerBase
{
    private readonly IMissionService _missions;
    private readonly IRuleValidationService _rules;

    public MissionsController(IMissionService missions, IRuleValidationService rules)
    {
        _missions = missions;
        _rules = rules;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var m = await _missions.GetMissionByIdAsync(id);
        return m == null ? NotFound() : Ok(m);
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> ByCategory(Guid categoryId)
    {
        var list = await _missions.GetMissionsByCategoryAsync(categoryId);
        return Ok(list);
    }

    [HttpGet("player/{playerId:guid}/available")]
    public async Task<IActionResult> Available(Guid playerId)
    {
        var list = await _missions.GetAvailableMissionsForPlayerAsync(playerId);
        return Ok(list);
    }

    [HttpGet("{id:guid}/rank-rules")]
    public async Task<IActionResult> RankRules(Guid id)
    {
        var rules = await _rules.GetApplicableRankRulesAsync(id);
        return Ok(rules);
    }

    public class CreateMissionRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid MissionCategoryId { get; set; }
        public Guid? ParentMissionId { get; set; }
        public int ExpReward { get; set; }
        public int ManaReward { get; set; }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMissionRequest dto)
    {
        var mission = new Mission
        {
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            MissionCategoryId = dto.MissionCategoryId,
            ParentMissionId = dto.ParentMissionId,
            ExpReward = dto.ExpReward,
            ManaReward = dto.ManaReward
        };
        mission = await _missions.CreateMissionAsync(mission);
        return CreatedAtAction(nameof(Get), new { id = mission.Id }, mission);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateMissionRequest dto)
    {
        var existing = await _missions.GetMissionByIdAsync(id);
        if (existing == null) return NotFound();
        existing.Title = dto.Title;
        existing.Description = dto.Description ?? string.Empty;
        existing.MissionCategoryId = dto.MissionCategoryId;
        existing.ParentMissionId = dto.ParentMissionId;
        existing.ExpReward = dto.ExpReward;
        existing.ManaReward = dto.ManaReward;
        await _missions.UpdateMissionAsync(existing);
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _missions.DeleteMissionAsync(id);
        return ok ? NoContent() : NotFound();
    }

    public record CompleteMissionRequest(Guid PlayerId, object? Proof);

    [HttpPost("{missionId:guid}/complete")]
    public async Task<IActionResult> Complete(Guid missionId, CompleteMissionRequest r)
    {
        var result = await _missions.CompleteMissionAsync(missionId, r.PlayerId, r.Proof);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

