namespace LctMonolith.Models;

public class Rank
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public int RequiredExperience { get; set; }
    public ICollection<RankRequiredMission> RequiredMissions { get; set; } = new List<RankRequiredMission>();
    public ICollection<RankRequiredCompetency> RequiredCompetencies { get; set; } = new List<RankRequiredCompetency>();
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
