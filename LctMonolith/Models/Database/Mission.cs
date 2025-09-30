namespace LctMonolith.Models.Database;

public class Mission
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MissionCategory? MissionCategory { get; set; }
    public Guid MissionCategoryId { get; set; } // changed from long
    public Mission? ParentMission { get; set; }
    public Guid? ParentMissionId { get; set; } // changed from long to nullable Guid
    public int ExpReward { get; set; }
    public int ManaReward { get; set; }
    public Guid DialogueId { get; set; }
    public Dialogue? Dialogue { get; set; }

    public ICollection<Mission> ChildMissions { get; set; } = new List<Mission>();
    public ICollection<PlayerMission> PlayerMissions { get; set; } = new List<PlayerMission>();
    public ICollection<MissionItemReward> MissionItemRewards { get; set; } = new List<MissionItemReward>();
    public ICollection<MissionSkillReward> MissionSkillRewards { get; set; } = new List<MissionSkillReward>();
    public ICollection<MissionRankRule> MissionRankRules { get; set; } = new List<MissionRankRule>();
    public ICollection<RankMissionRule> RankMissionRules { get; set; } = new List<RankMissionRule>();
}
