using LctMonolith.Models.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Database.Data;

/// <summary>
/// Development database seeder for initial ranks, competencies, sample store items.
/// Idempotent: checks existence before inserting.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await db.Database.EnsureCreatedAsync(ct);

        if (!await db.Ranks.AnyAsync(ct))
        {
            var ranks = new List<Rank>
            {
                new() { Title = "Искатель", ExpNeeded = 0 },
                new() { Title = "Пилот-кандидат", ExpNeeded = 500 },
                new() { Title = "Принятый в экипаж", ExpNeeded = 1500 }
            };
            db.Ranks.AddRange(ranks);
            Log.Information("Seeded {Count} ranks", ranks.Count);
        }

        if (!await db.Skills.AnyAsync(ct))
        {
            var comps = new[]
            {
                "Вера в дело","Стремление к большему","Общение","Аналитика","Командование","Юриспруденция","Трёхмерное мышление","Базовая экономика","Основы аэронавигации"
            }.Select(n => new Skill { Title = n });
            db.Skills.AddRange(comps);
            Log.Information("Seeded competencies");
        }

        if (!await db.StoreItems.AnyAsync(ct))
        {
            db.StoreItems.AddRange(new StoreItem { Name = "Футболка Алабуга", Price = 100 }, new StoreItem { Name = "Брелок Буран", Price = 50 });
            Log.Information("Seeded store items");
        }

        await db.SaveChangesAsync(ct);
    }
}
