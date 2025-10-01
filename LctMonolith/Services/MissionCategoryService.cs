using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services
{
    public class MissionCategoryService : IMissionCategoryService
    {
        private readonly IUnitOfWork _uow;

        public MissionCategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<MissionCategory?> GetCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                return await _uow.MissionCategories.GetByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetCategoryByIdAsync failed {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<MissionCategory?> GetCategoryByTitleAsync(string title)
        {
            try
            {
                return await _uow.MissionCategories.Query(c => c.Title == title).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetCategoryByTitleAsync failed {Title}", title);
                throw;
            }
        }

        public async Task<IEnumerable<MissionCategory>> GetAllCategoriesAsync()
        {
            try
            {
                return await _uow.MissionCategories.Query().OrderBy(c => c.Title).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllCategoriesAsync failed");
                throw;
            }
        }

        public async Task<MissionCategory> CreateCategoryAsync(MissionCategory category)
        {
            try
            {
                category.Id = Guid.NewGuid();
                await _uow.MissionCategories.AddAsync(category);
                await _uow.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CreateCategoryAsync failed {Title}", category.Title);
                throw;
            }
        }

        public async Task<MissionCategory> UpdateCategoryAsync(MissionCategory category)
        {
            try
            {
                _uow.MissionCategories.Update(category);
                await _uow.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "UpdateCategoryAsync failed {Id}", category.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                var c = await _uow.MissionCategories.GetByIdAsync(categoryId);
                if (c == null) { return false; }
                _uow.MissionCategories.Remove(c);
                await _uow.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DeleteCategoryAsync failed {CategoryId}", categoryId);
                throw;
            }
        }
    }
}
