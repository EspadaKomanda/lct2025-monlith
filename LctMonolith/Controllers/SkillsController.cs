using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;
using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _skillService.GetAllSkillsAsync();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var s = await _skillService.GetSkillByIdAsync(id);
        return s == null ? NotFound() : Ok(s);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateSkillDto dto)
    {
        var skill = await _skillService.CreateSkillAsync(new Skill { Title = dto.Title });
        return CreatedAtAction(nameof(Get), new { id = skill.Id }, skill);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateSkillDto dto)
    {
        var s = await _skillService.GetSkillByIdAsync(id);
        if (s == null) return NotFound();
        s.Title = dto.Title;
        await _skillService.UpdateSkillAsync(s);
        return Ok(s);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _skillService.DeleteSkillAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("player/{playerId:guid}")]
    public async Task<IActionResult> PlayerSkills(Guid playerId)
    {
        var list = await _skillService.GetPlayerSkillsAsync(playerId);
        return Ok(list);
    }

    public record UpdatePlayerSkillRequest(int Level);

    [HttpPost("player/{playerId:guid}/{skillId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePlayerSkill(Guid playerId, Guid skillId, UpdatePlayerSkillRequest r)
    {
        var ps = await _skillService.UpdatePlayerSkillAsync(playerId, skillId, r.Level);
        return Ok(new { ps.PlayerId, ps.SkillId, ps.Score });
    }
}

