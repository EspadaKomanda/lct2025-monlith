using Microsoft.AspNetCore.Identity;

namespace LctMonolith.Models.Database;

public class AppUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int Experience { get; set; }
    public int Mana { get; set; }
    public Guid? RankId { get; set; }
    public Rank? Rank { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<PlayerSkill> Competencies { get; set; } = new List<PlayerSkill>();
    public ICollection<PlayerMission> Missions { get; set; } = new List<PlayerMission>();
    public ICollection<UserInventoryItem> Inventory { get; set; } = new List<UserInventoryItem>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<EventLog> Events { get; set; } = new List<EventLog>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
