using App.DTO.Category;
using App.DTO.Product;
using App.Helpers;
using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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
            return await _context.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(i => i.CategoryId == id);
        }

        public async Task<Category> UpdateAsync(int id, UpdateCategoryRequest categoryDTO)
        {
            var existringCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (existringCategory == null) return null;

            existringCategory.Name = categoryDTO.Name;

            await _context.SaveChangesAsync();
            return existringCategory;
        }

        public async Task<string> Import(IFormFile file, int id)
        {
            var products = new List<Product>();
            using (var stream = new MemoryStream())
            {

                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy worksheet đầu tiên
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Bắt đầu từ hàng 2
                    {
                        var productDTO = new ProductDTO
                        {
                            Name = worksheet.Cells[row, 1].Text, // Cột A
                            Description = worksheet.Cells[row, 2].Text, // Cột B
                            Price = decimal.Parse(worksheet.Cells[row, 3].Text), // Cột C
                            Image = await ImageHelper.ConvertImageToBase64Async(worksheet.Cells[row, 4].Text), // Cột D
                            CategoryName = ""
                        };
                        products.Add(ToProduct(productDTO, id));
                    }
                }
            }
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return "Thêm thành công";
        }
        private Product ToProduct(ProductDTO dto, int id)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = new Random().Next(1, 25),
                CategoryId = id,
                Image = dto.Image,
                CreatedAt = DateTime.Now
            };
        }

    }

}