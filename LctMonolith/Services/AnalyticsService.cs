using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        try
        {
            var totalUsers = await _uow.Users.Query().CountAsync(ct);
            var totalMissions = await _uow.Missions.Query().CountAsync(ct);
            var totalStoreItems = await _uow.StoreItems.Query().CountAsync(ct);
            var totalExperience = await _uow.Players.Query().SumAsync(p => (long)p.Experience, ct);
            var completedMissions = await _uow.PlayerMissions.Query(pm => pm.Completed != null).CountAsync(ct);
            return new AnalyticsSummary
            {
                TotalUsers = totalUsers,
                TotalMissions = totalMissions,
                TotalStoreItems = totalStoreItems,
                TotalExperience = totalExperience,
                CompletedMissions = completedMissions
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to build analytics summary");
            throw;
        }
    }
}
