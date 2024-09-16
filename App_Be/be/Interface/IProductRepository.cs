using App.DTO.Product;
using App.Helpers;
using App.Models;

namespace App.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(QueryObject query);
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product ProductModel);
        Task<List<Product>> Import(IFormFile file);
        Task<Product> UpdateAsync(int id, UpdateProductRequest ProductDTO);
        Task<Product> DeledeAsync(int id);
    }

}