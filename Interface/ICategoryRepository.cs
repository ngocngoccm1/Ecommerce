using App.Models;

namespace App.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();

    }

}