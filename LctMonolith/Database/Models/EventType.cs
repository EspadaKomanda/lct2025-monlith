namespace LctMonolith.Models.Database;

public enum EventType
{
    SkillProgress = 1,
    MissionStatusChanged = 2,
    RankChanged = 3,
    ItemPurchased = 4,
    ArtifactObtained = 5,
    RewardGranted = 6,
    ProfileChanged = 7,
    AuthCredentialsChanged = 8,
    ItemReturned = 9,
    ItemSold = 10
}

#if false
// Moved to Models/EventType.cs
#endif
