namespace LctMonolith.Models.Database;

public class Player
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RankId { get; set; }
    public Rank? Rank { get; set; }
    public int Experience { get; set; }
    public int Mana { get; set; }
    public ICollection<PlayerMission> PlayerMissions { get; set; } = new List<PlayerMission>();
    public ICollection<PlayerSkill> PlayerSkills { get; set; } = new List<PlayerSkill>();
}
