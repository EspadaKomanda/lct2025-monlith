using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LctMonolith.Services;

/// <summary>
/// Provides aggregated analytics metrics for dashboards.
/// </summary>
public class AnalyticsService : IAnalyticsService
{
    private readonly IUnitOfWork _uow;
    public AnalyticsService(IUnitOfWork uow) => _uow = uow;

    public async Task<AnalyticsSummary> GetSummaryAsync(CancellationToken ct = default)
    {
        var totalUsers = await _uow.Users.Query().CountAsync(ct);
        var totalMissions = await _uow.Missions.Query().CountAsync(ct);
        var totalStoreItems = await _uow.StoreItems.Query().CountAsync(ct);
        var totalExperience = await _uow.Users.Query().SumAsync(u => (long)u.Experience, ct);
        return new AnalyticsSummary
        {
            TotalUsers = totalUsers,
            TotalMissions = totalMissions,
            TotalStoreItems = totalStoreItems,
            TotalExperience = totalExperience
        };
    }
}
