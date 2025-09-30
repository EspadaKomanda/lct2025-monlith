namespace LctMonolith.Models.Database;

public class MissionCategory
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public ICollection<Mission> Missions { get; set; } = new List<Mission>();
}
