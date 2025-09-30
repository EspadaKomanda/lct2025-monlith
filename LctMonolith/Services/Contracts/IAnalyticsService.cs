using LctMonolith.Services.Models;

namespace LctMonolith.Services;

public interface IAnalyticsService
{
    Task<AnalyticsSummary> GetSummaryAsync(CancellationToken ct = default);
}
