using LctMonolith.Database.UnitOfWork; 
using LctMonolith.Models.Database; 
using LctMonolith.Models.DTO; 
using LctMonolith.Services.Interfaces; 
using Microsoft.EntityFrameworkCore; 
using Serilog; 

namespace LctMonolith.Services; 

public class ProgressTrackingService : IProgressTrackingService 
{ 
    private readonly IUnitOfWork _uow; 
    private readonly IMissionService _missionService; 

    public ProgressTrackingService(IUnitOfWork uow, IMissionService missionService) 
    { 
        _uow = uow; 
        _missionService = missionService; 
    } 

    public async Task<PlayerMission> StartMissionAsync(Guid missionId, Guid playerId) 
    { 
        try 
        { 
            var existing = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == missionId).FirstOrDefaultAsync(); 
            if (existing != null) return existing; 
            var pm = new PlayerMission { Id = Guid.NewGuid(), PlayerId = playerId, MissionId = missionId, Started = DateTime.UtcNow, ProgressPercent = 0 }; 
            await _uow.PlayerMissions.AddAsync(pm); 
            await _uow.SaveChangesAsync(); 
            return pm; 
        } 
        catch (Exception ex) { Log.Error(ex, "StartMissionAsync failed {MissionId} {PlayerId}", missionId, playerId); throw; } 
    } 

    public async Task<PlayerMission> UpdateMissionProgressAsync(Guid playerMissionId, int progressPercentage, object? proof = null) 
    { 
        try 
        { 
            if (progressPercentage is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(progressPercentage)); 
            var pm = await _uow.PlayerMissions.GetByIdAsync(playerMissionId) ?? throw new KeyNotFoundException("PlayerMission not found"); 
            if (pm.Completed != null) return pm; 
            pm.ProgressPercent = progressPercentage; 
            if (progressPercentage == 100 && pm.Completed == null) 
            { 
                // Complete mission through mission service to allocate rewards, etc. 
                await _missionService.CompleteMissionAsync(pm.MissionId, pm.PlayerId); 
            } 
            await _uow.SaveChangesAsync(); 
            return pm; 
        } 
        catch (Exception ex) { Log.Error(ex, "UpdateMissionProgressAsync failed {PlayerMissionId}", playerMissionId); throw; } 
    } 

    public async Task<PlayerMission> CompleteMissionAsync(Guid playerMissionId, object? proof = null) 
    { 
        try 
        { 
            var pm = await _uow.PlayerMissions.GetByIdAsync(playerMissionId) ?? throw new KeyNotFoundException("PlayerMission not found"); 
            if (pm.Completed != null) return pm; 
            await _missionService.CompleteMissionAsync(pm.MissionId, pm.PlayerId, proof); 
            await _uow.SaveChangesAsync(); 
            return pm; 
        } 
        catch (Exception ex) { Log.Error(ex, "CompleteMissionAsync (progress) failed {PlayerMissionId}", playerMissionId); throw; } 
    } 

    public async Task<IEnumerable<PlayerMission>> GetPlayerMissionsAsync(Guid playerId) 
    { 
        try { return await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId).ToListAsync(); } 
        catch (Exception ex) { Log.Error(ex, "GetPlayerMissionsAsync failed {PlayerId}", playerId); throw; } 
    } 

    public async Task<PlayerMission?> GetPlayerMissionAsync(Guid playerId, Guid missionId) 
    { 
        try { return await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == missionId).FirstOrDefaultAsync(); } 
        catch (Exception ex) { Log.Error(ex, "GetPlayerMissionAsync failed {MissionId} {PlayerId}", missionId, playerId); throw; } 
    } 

    public async Task<PlayerProgress> GetPlayerOverallProgressAsync(Guid playerId) 
    { 
        try 
        { 
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found"); 
            var missions = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId).ToListAsync(); 
            var completed = missions.Count(m => m.Completed != null); 
            var totalMissions = await _uow.Missions.Query().CountAsync(); 
            var skillLevels = await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId, null, ps => ps.Skill) 
                .ToDictionaryAsync(ps => ps.Skill.Title, ps => ps.Score); 
            return new PlayerProgress 
            { 
                PlayerId = playerId, 
                PlayerName = playerId.ToString(), 
                CurrentRank = await _uow.Ranks.GetByIdAsync(player.RankId), 
                TotalExperience = player.Experience, 
                TotalMana = player.Mana, 
                CompletedMissions = completed, 
                TotalAvailableMissions = totalMissions, 
                SkillLevels = skillLevels 
            }; 
        } 
        catch (Exception ex) { Log.Error(ex, "GetPlayerOverallProgressAsync failed {PlayerId}", playerId); throw; } 
    } 
}
