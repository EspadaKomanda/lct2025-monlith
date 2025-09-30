
using LctMonolith.Database.Data;
using LctMonolith.Database.Repositories;
using LctMonolith.Models.Database;
using Microsoft.EntityFrameworkCore.Storage;
using EventLog = LctMonolith.Models.Database.EventLog;

namespace LctMonolith.Database.UnitOfWork;

/// <summary>
/// Unit of Work implementation encapsulating repositories and DB transaction scope.
/// </summary>
public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly AppDbContext _ctx;
    private IDbContextTransaction? _tx;

    public UnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    private IGenericRepository<AppUser>? _users;
    private IGenericRepository<Rank>? _ranks;
    private IGenericRepository<RankMissionRule>? _rankMissionRules;
    private IGenericRepository<RankSkillRule>? _rankSkillRules;
    private IGenericRepository<Mission>? _missions;
    private IGenericRepository<PlayerMission>? _playerMissions;
    private IGenericRepository<MissionSkillReward>? _missionSkillRewards;
    private IGenericRepository<Skill>? _skills;
    private IGenericRepository<PlayerSkill>? _playerSkills;
    private IGenericRepository<StoreItem>? _storeItems;
    private IGenericRepository<UserInventoryItem>? _userInventoryItems;
    private IGenericRepository<Transaction>? _transactions;
    private IGenericRepository<EventLog>? _eventLogs;
    private IGenericRepository<RefreshToken>? _refreshTokens;
    private IGenericRepository<Notification>? _notifications;

    public IGenericRepository<AppUser> Users => _users ??= new GenericRepository<AppUser>(_ctx);
    public IGenericRepository<Rank> Ranks => _ranks ??= new GenericRepository<Rank>(_ctx);
    public IGenericRepository<RankMissionRule> RankMissionRules => _rankMissionRules ??= new GenericRepository<RankMissionRule>(_ctx);
    public IGenericRepository<RankSkillRule> RankSkillRules => _rankSkillRules ??= new GenericRepository<RankSkillRule>(_ctx);
    public IGenericRepository<Mission> Missions => _missions ??= new GenericRepository<Mission>(_ctx);
    public IGenericRepository<PlayerMission> PlayerMissions => _playerMissions ??= new GenericRepository<PlayerMission>(_ctx);
    public IGenericRepository<MissionSkillReward> MissionSkillRewards => _missionSkillRewards ??= new GenericRepository<MissionSkillReward>(_ctx);
    public IGenericRepository<Skill> Skills => _skills ??= new GenericRepository<Skill>(_ctx);
    public IGenericRepository<PlayerSkill> PlayerSkills => _playerSkills ??= new GenericRepository<PlayerSkill>(_ctx);
    public IGenericRepository<StoreItem> StoreItems => _storeItems ??= new GenericRepository<StoreItem>(_ctx);
    public IGenericRepository<UserInventoryItem> UserInventoryItems => _userInventoryItems ??= new GenericRepository<UserInventoryItem>(_ctx);
    public IGenericRepository<Transaction> Transactions => _transactions ??= new GenericRepository<Transaction>(_ctx);
    public IGenericRepository<EventLog> EventLogs => _eventLogs ??= new GenericRepository<EventLog>(_ctx);
    public IGenericRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new GenericRepository<RefreshToken>(_ctx);
    public IGenericRepository<Notification> Notifications => _notifications ??= new GenericRepository<Notification>(_ctx);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_tx != null) throw new InvalidOperationException("Transaction already started");
        _tx = await _ctx.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_tx == null) return;
        try
        {
            await _ctx.SaveChangesAsync(ct);
            await _tx.CommitAsync(ct);
        }
        catch
        {
            await RollbackAsync(ct);
            throw;
        }
        finally
        {
            await _tx.DisposeAsync();
            _tx = null;
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_tx == null) return;
        await _tx.RollbackAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_tx != null) await _tx.DisposeAsync();
    }
}
