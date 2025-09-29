namespace LctMonolith.Models;

public class Artifact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public ArtifactRarity Rarity { get; set; }
    public ICollection<UserArtifact> Users { get; set; } = new List<UserArtifact>();
    public ICollection<MissionArtifactReward> MissionRewards { get; set; } = new List<MissionArtifactReward>();
}

