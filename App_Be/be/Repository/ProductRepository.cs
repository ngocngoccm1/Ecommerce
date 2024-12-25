using System.Drawing;
using App.DTO.Product;
using App.Helpers;
using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace App.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product> DeledeAsync(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (productModel == null) return null;
            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
        public async Task<List<Product>> GetAllProductAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetAllAsync(QueryObject query)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CategoryId.ToString()))
            {
                products = products.Where(s => s.Category.CategoryId == query.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(s => s.Name.Contains(query.Name));
            }

            if (query.Isdescending)
            {
                products = products.OrderByDescending(s => s.Price);
            }
            else if (!query.Isdescending)
            {
                products = products.OrderBy(s => s.Price);
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                                .Include(p => p.Category)

                                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<List<Product>> Import(IFormFile file)
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
                            CategoryName = worksheet.Cells[row, 5].Text // Gán danh mục
                                                                        // Gán danh mục
                        };
                        products.Add(ToProduct(productDTO));
                    }
                }
            }
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return products;
        }
        private Product ToProduct(ProductDTO dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = new Random().Next(1, 25),
                CategoryId = GetCategoryIdByName(dto.CategoryName),
                Image = dto.Image,
                CreatedAt = DateTime.Now
            };
        }
        private int GetCategoryIdByName(string categoryName)
        {
            var categoryId = _context.Categories
                                     .Where(c => c.Name == categoryName)
                                     .Select(c => c.CategoryId)
                                     .FirstOrDefault();
            return categoryId; // Trả về ID nếu tìm thấy, ngược lại trả về null
        }

        public async Task<Product> UpdateAsync(int id, UpdateProductRequest ProductDTO)
        {
            var existringProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (existringProduct == null) return null;

            existringProduct.Name = ProductDTO.Name;
            existringProduct.Description = ProductDTO.Description;
            existringProduct.Price = ProductDTO.Price;
            existringProduct.Stock = ProductDTO.Stock;
            existringProduct.Image = ImageHelper.EncodeImageToBase64(ProductDTO.Image);
            existringProduct.CategoryId = ProductDTO.CategoryId;

            await _context.SaveChangesAsync();
            return existringProduct;
        }


    }
}