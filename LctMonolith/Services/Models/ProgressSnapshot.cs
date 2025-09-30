namespace LctMonolith.Services.Models;

public class ProgressSnapshot
{
    public int Experience { get; set; }
    public int Mana { get; set; }
    public Guid? CurrentRankId { get; set; }
    public string? CurrentRankName { get; set; }
    public Guid? NextRankId { get; set; }
    public string? NextRankName { get; set; }
    public int? RequiredExperienceForNextRank { get; set; }
    public int? ExperienceRemaining => RequiredExperienceForNextRank.HasValue ? Math.Max(0, RequiredExperienceForNextRank.Value - Experience) : null;
    public List<Guid> OutstandingMissionIds { get; set; } = new();
    public List<OutstandingCompetency> OutstandingCompetencies { get; set; } = new();
}

public class OutstandingCompetency
{
    public Guid CompetencyId { get; set; }
    public string? CompetencyName { get; set; }
    public int RequiredLevel { get; set; }
    public int CurrentLevel { get; set; }
}
