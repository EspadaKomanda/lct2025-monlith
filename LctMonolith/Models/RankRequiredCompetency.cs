namespace LctMonolith.Models;

public class RankRequiredCompetency
{
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    public Guid CompetencyId { get; set; }
    public Competency Competency { get; set; } = null!;
    public int MinLevel { get; set; }
}
