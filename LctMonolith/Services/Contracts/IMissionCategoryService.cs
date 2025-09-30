using LctMonolith.Models.Database;

namespace LctMonolith.Services.Interfaces;

public interface IMissionCategoryService
{
    // CRUD should be enough
    Task<MissionCategory?> GetCategoryByIdAsync(Guid categoryId);
    Task<MissionCategory?> GetCategoryByTitleAsync(string title);
    Task<IEnumerable<MissionCategory>> GetAllCategoriesAsync();
    Task<MissionCategory> CreateCategoryAsync(MissionCategory category);
    Task<MissionCategory> UpdateCategoryAsync(MissionCategory category);
    Task<bool> DeleteCategoryAsync(Guid categoryId);
}
