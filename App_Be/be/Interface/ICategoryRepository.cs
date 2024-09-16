using App.DTO.Category;
using App.Models;

namespace App.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category categoryModel);
        Task<Category> UpdateAsync(int id, UpdateCategoryRequest categoryDTO);
        Task<Category> DeledeAsync(int id);
        Task<string> Import(IFormFile file, int id);
    }

}