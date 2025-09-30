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
    IGenericRepository<Player> Players { get; }
    IGenericRepository<MissionCategory> MissionCategories { get; } // added
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
    IGenericRepository<MissionItemReward> MissionItemRewards { get; } // added
    IGenericRepository<MissionRankRule> MissionRankRules { get; } // added
    IGenericRepository<Dialogue> Dialogues { get; }
    IGenericRepository<DialogueMessage> DialogueMessages { get; }
    IGenericRepository<DialogueMessageResponseOption> DialogueMessageResponseOptions { get; }
    IGenericRepository<Profile> Profiles { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
