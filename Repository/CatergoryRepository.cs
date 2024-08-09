using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.CateroryRepository
{
    public class CatergoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CatergoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<List<Category>> GetAllAsync()
        {
            return _context.Categories.ToListAsync();

        }
    }

}