using LctMonolith.Database.Repositories;
using LctMonolith.Models.Database;
using EventLog = LctMonolith.Models.Database.EventLog;

namespace LctMonolith.Database.UnitOfWork;

/// <summary>
/// Unit of Work aggregates repositories and transaction boundary.
/// </summary>
public interface IUnitOfWork
{
    IGenericRepository<AppUser> Users { get; }
    IGenericRepository<Rank> Ranks { get; }
    IGenericRepository<RankMissionRule> RankMissionRules { get; }
    IGenericRepository<RankSkillRule> RankSkillRules { get; }
    IGenericRepository<Mission> Missions { get; }
    IGenericRepository<PlayerMission> PlayerMissions { get; }
    IGenericRepository<MissionSkillReward> MissionSkillRewards { get; }
    IGenericRepository<Skill> Skills { get; }
    IGenericRepository<PlayerSkill> PlayerSkills { get; }
    IGenericRepository<StoreItem> StoreItems { get; }
    IGenericRepository<UserInventoryItem> UserInventoryItems { get; }
    IGenericRepository<Transaction> Transactions { get; }
    IGenericRepository<EventLog> EventLogs { get; }
    IGenericRepository<RefreshToken> RefreshTokens { get; }
    IGenericRepository<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
