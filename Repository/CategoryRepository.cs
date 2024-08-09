using App.DTO.Category;
using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category categoryModel)
        {
            await _context.Categories.AddAsync(categoryModel);
            await _context.SaveChangesAsync();
            return categoryModel;
        }

        public async Task<Category> DeledeAsync(int id)
        {
            var categoryModel = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (categoryModel == null) return null;
            _context.Categories.Remove(categoryModel);
            await _context.SaveChangesAsync();
            return categoryModel;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> UpdateAsync(int id, UpdateCategoryRequest categoryDTO)
        {
            var existringCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (existringCategory == null) return null;

            existringCategory.Name = categoryDTO.Name;

            await _context.SaveChangesAsync();
            return existringCategory;
        }
    }

}