using App.DTO.Product;
using App.Interface;
using App.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        public async Task<Product> UpdateAsync(int id, UpdateProductRequest ProductDTO)
        {
            var existringProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (existringProduct == null) return null;

            existringProduct.Name = ProductDTO.Name;
            existringProduct.Description = ProductDTO.Description;
            existringProduct.Price = ProductDTO.Price;
            existringProduct.Stock = ProductDTO.Stock;
            existringProduct.CategoryId = ProductDTO.CategoryId;

            await _context.SaveChangesAsync();
            return existringProduct;
        }

    }

}