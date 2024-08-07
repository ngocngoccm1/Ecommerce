using App.DTO.Product;
using App.Models;

namespace App.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product ProductModel);
        Task<Product> UpdateAsync(int id, UpdateProductRequest ProductDTO);
        Task<Product> DeledeAsync(int id);
    }

}