using LctMonolith.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventLog = LctMonolith.Models.Database.EventLog;

namespace LctMonolith.Database.Data;

/// <summary>
/// Main EF Core database context for gamification module (PostgreSQL provider expected).
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Rank related entities
    public DbSet<Rank> Ranks => Set<Rank>();
    public DbSet<RankMissionRule> RankMissionRules => Set<RankMissionRule>();
    public DbSet<RankSkillRule> RankSkillRules => Set<RankSkillRule>();

    // Mission related entities
    public DbSet<MissionCategory> MissionCategories => Set<MissionCategory>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<PlayerMission> PlayerMissions => Set<PlayerMission>();
    public DbSet<MissionSkillReward> MissionSkillRewards => Set<MissionSkillReward>();
    public DbSet<MissionItemReward> MissionItemRewards => Set<MissionItemReward>();
    public DbSet<MissionRankRule> MissionRankRules => Set<MissionRankRule>();

    // Skill related entities
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<PlayerSkill> PlayerSkills => Set<PlayerSkill>();

    // Dialogue related entities
    public DbSet<Dialogue> Dialogues => Set<Dialogue>();
    public DbSet<DialogueMessage> DialogueMessages => Set<DialogueMessage>();
    public DbSet<DialogueMessageResponseOption> DialogueMessageResponseOptions => Set<DialogueMessageResponseOption>();

    // Store and inventory
    public DbSet<StoreItem> StoreItems => Set<StoreItem>();
    public DbSet<UserInventoryItem> UserInventoryItems => Set<UserInventoryItem>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    // System entities
    public DbSet<EventLog> EventLogs => Set<EventLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();

    // Core profile / player chain
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Player configuration
        b.Entity<Player>()
            .HasIndex(p => p.UserId)
            .IsUnique();
        b.Entity<Player>()
            .HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<Player>(p => p.UserId)
            .IsRequired();

        // Rank configurations
        b.Entity<Rank>()
            .HasIndex(r => r.ExpNeeded)
            .IsUnique();
        b.Entity<Rank>()
            .HasIndex(r => r.Title)
            .IsUnique();

        // Skill configurations
        b.Entity<Skill>()
            .HasIndex(s => s.Title)
            .IsUnique();

        // MissionCategory configurations
        b.Entity<MissionCategory>()
            .HasIndex(mc => mc.Title)
            .IsUnique();

        // Mission configurations
        b.Entity<Mission>()
            .HasOne(m => m.MissionCategory)
            .WithMany(mc => mc.Missions)
            .HasForeignKey(m => m.MissionCategoryId)
            .IsRequired();
        b.Entity<Mission>()
            .HasOne(m => m.ParentMission)
            .WithMany(m => m.ChildMissions)
            .HasForeignKey(m => m.ParentMissionId)
            .IsRequired(false);
        // Dialogue relationship for Mission
        b.Entity<Mission>()
            .HasOne(m => m.Dialogue)
            .WithOne(d => d.Mission)
            .HasForeignKey<Mission>(m => m.DialogueId)
            .IsRequired(false);

        // MissionRankRule configurations
        b.Entity<MissionRankRule>()
            .HasOne(mrr => mrr.Mission)
            .WithMany(m => m.MissionRankRules)
            .HasForeignKey(mrr => mrr.MissionId);
        b.Entity<MissionRankRule>()
            .HasOne(mrr => mrr.Rank)
            .WithMany(r => r.MissionRankRules)
            .HasForeignKey(mrr => mrr.RankId);

        // MissionSkillReward configurations
        b.Entity<MissionSkillReward>()
            .HasKey(x => new { x.MissionId, x.SkillId });
        b.Entity<MissionSkillReward>()
            .HasOne(msr => msr.Mission)
            .WithMany(m => m.MissionSkillRewards)
            .HasForeignKey(msr => msr.MissionId);
        b.Entity<MissionSkillReward>()
            .HasOne(msr => msr.Skill)
            .WithMany(s => s.MissionSkillRewards)
            .HasForeignKey(msr => msr.SkillId);

        // MissionItemReward configurations
        b.Entity<MissionItemReward>()
            .HasOne(mir => mir.Mission)
            .WithMany(m => m.MissionItemRewards)
            .HasForeignKey(mir => mir.MissionId);

        // RankMissionRule composite key
        b.Entity<RankMissionRule>().HasKey(x => new { x.RankId, x.MissionId });
        b.Entity<RankMissionRule>()
            .HasOne(rmr => rmr.Rank)
            .WithMany(r => r.RankMissionRules)
            .HasForeignKey(rmr => rmr.RankId);
        b.Entity<RankMissionRule>()
            .HasOne(rmr => rmr.Mission)
            .WithMany(m => m.RankMissionRules)
            .HasForeignKey(rmr => rmr.MissionId);

        // RankSkillRule composite key
        b.Entity<RankSkillRule>().HasKey(x => new { x.RankId, x.SkillId });
        b.Entity<RankSkillRule>()
            .HasOne(rsr => rsr.Rank)
            .WithMany(r => r.RankSkillRules)
            .HasForeignKey(rsr => rsr.RankId);
        b.Entity<RankSkillRule>()
            .HasOne(rsr => rsr.Skill)
            .WithMany(s => s.RankSkillRules)
            .HasForeignKey(rsr => rsr.SkillId);

        // PlayerSkill composite key
        b.Entity<PlayerSkill>().HasKey(x => new { x.PlayerId, x.SkillId });
        b.Entity<PlayerSkill>()
            .HasOne(ps => ps.Player)
            .WithMany(p => p.PlayerSkills)
            .HasForeignKey(ps => ps.PlayerId);
        b.Entity<PlayerSkill>()
            .HasOne(ps => ps.Skill)
            .WithMany(s => s.PlayerSkills)
            .HasForeignKey(ps => ps.SkillId);

        // PlayerMission composite key
        b.Entity<PlayerMission>().HasKey(x => new { x.PlayerId, x.MissionId });
        b.Entity<PlayerMission>()
            .HasOne(pm => pm.Player)
            .WithMany(p => p.PlayerMissions)
            .HasForeignKey(pm => pm.PlayerId);
        b.Entity<PlayerMission>()
            .HasOne(pm => pm.Mission)
            .WithMany(m => m.PlayerMissions)
            .HasForeignKey(pm => pm.MissionId);

        // Dialogue configurations
        b.Entity<Dialogue>()
            .HasOne(d => d.Mission)
            .WithOne(m => m.Dialogue)
            .HasForeignKey<Dialogue>(d => d.MissionId)
            .IsRequired();

        // DialogueMessage configurations
        b.Entity<DialogueMessage>()
            .HasOne(dm => dm.InitialDialogue)
            .WithMany()
            .HasForeignKey(dm => dm.InitialDialogueId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<DialogueMessage>()
            .HasOne(dm => dm.InterimDialogue)
            .WithMany()
            .HasForeignKey(dm => dm.InterimDialogueId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<DialogueMessage>()
            .HasOne(dm => dm.EndDialogue)
            .WithMany()
            .HasForeignKey(dm => dm.EndDialogueId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // DialogueMessageResponseOption configurations
        b.Entity<DialogueMessageResponseOption>()
            .HasOne(dmro => dmro.ParentDialogueMessage)
            .WithMany(dm => dm.DialogueMessageResponseOptions)
            .HasForeignKey(dmro => dmro.ParentDialogueMessageId)
            .IsRequired();
        b.Entity<DialogueMessageResponseOption>()
            .HasOne(dmro => dmro.DestinationDialogueMessage)
            .WithMany()
            .HasForeignKey(dmro => dmro.DestinationDialogueMessageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Refresh token index unique
        b.Entity<RefreshToken>().HasIndex(x => x.Token).IsUnique();

        // ---------- Performance indexes ----------
        b.Entity<PlayerSkill>().HasIndex(ps => ps.SkillId);
        b.Entity<EventLog>().HasIndex(e => new { e.UserId, e.Type, e.CreatedAt });
        b.Entity<StoreItem>().HasIndex(i => i.IsActive);
        b.Entity<Transaction>().HasIndex(t => new { t.UserId, t.CreatedAt });
        b.Entity<Notification>().HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt });
    }
}
