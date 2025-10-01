using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _uow;
    public SkillService(IUnitOfWork uow) { _uow = uow; }

    public async Task<Skill?> GetSkillByIdAsync(Guid skillId)
    {
        try { return await _uow.Skills.GetByIdAsync(skillId); }
        catch (Exception ex) { Log.Error(ex, "GetSkillByIdAsync failed {SkillId}", skillId); throw; }
    }
    public async Task<Skill?> GetSkillByTitleAsync(string title)
    {
        try { return await _uow.Skills.Query(s => s.Title == title).FirstOrDefaultAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetSkillByTitleAsync failed {Title}", title); throw; }
    }
    public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
    {
        try { return await _uow.Skills.Query().OrderBy(s => s.Title).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetAllSkillsAsync failed"); throw; }
    }
    public async Task<Skill> CreateSkillAsync(Skill skill)
    {
        try { skill.Id = Guid.NewGuid(); await _uow.Skills.AddAsync(skill); await _uow.SaveChangesAsync(); return skill; }
        catch (Exception ex) { Log.Error(ex, "CreateSkillAsync failed {Title}", skill.Title); throw; }
    }
    public async Task<Skill> UpdateSkillAsync(Skill skill)
    {
        try { _uow.Skills.Update(skill); await _uow.SaveChangesAsync(); return skill; }
        catch (Exception ex) { Log.Error(ex, "UpdateSkillAsync failed {SkillId}", skill.Id); throw; }
    }
    public async Task<bool> DeleteSkillAsync(Guid skillId)
    {
        try { var skill = await _uow.Skills.GetByIdAsync(skillId); if (skill == null) { return false; } _uow.Skills.Remove(skill); await _uow.SaveChangesAsync(); return true; }
        catch (Exception ex) { Log.Error(ex, "DeleteSkillAsync failed {SkillId}", skillId); throw; }
    }
    public async Task<PlayerSkill> UpdatePlayerSkillAsync(Guid playerId, Guid skillId, int level)
    {
        try { var ps = await _uow.PlayerSkills.Query(x => x.PlayerId == playerId && x.SkillId == skillId).FirstOrDefaultAsync(); if (ps == null) { ps = new PlayerSkill { Id = Guid.NewGuid(), PlayerId = playerId, SkillId = skillId, Score = level }; await _uow.PlayerSkills.AddAsync(ps); } else { ps.Score = level; _uow.PlayerSkills.Update(ps); } await _uow.SaveChangesAsync(); return ps; }
        catch (Exception ex) { Log.Error(ex, "UpdatePlayerSkillAsync failed {PlayerId} {SkillId}", playerId, skillId); throw; }
    }
    public async Task<IEnumerable<PlayerSkill>> GetPlayerSkillsAsync(Guid playerId)
    {
        try { return await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId, null, ps => ps.Skill).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetPlayerSkillsAsync failed {PlayerId}", playerId); throw; }
    }
    public async Task<int> GetPlayerSkillLevelAsync(Guid playerId, Guid skillId)
    {
        try { var ps = await _uow.PlayerSkills.Query(x => x.PlayerId == playerId && x.SkillId == skillId).FirstOrDefaultAsync(); return ps?.Score ?? 0; }
        catch (Exception ex) { Log.Error(ex, "GetPlayerSkillLevelAsync failed {PlayerId} {SkillId}", playerId, skillId); throw; }
    }
}
