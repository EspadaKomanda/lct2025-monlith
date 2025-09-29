namespace LctMonolith.Models;

public class UserCompetency
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid CompetencyId { get; set; }
    public Competency Competency { get; set; } = null!;
    public int Level { get; set; }
    public int ProgressPoints { get; set; }
}
