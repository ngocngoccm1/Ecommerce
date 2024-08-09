using App.DTO.Product;
using App.Models;

namespace App.Mappers
{
    public static class ProductMappers
    {
        public static ProductDTO ToProductDto(this Product ProductModel)
        {
            return new ProductDTO
            {
                ProductId = ProductModel.ProductId,
                Name = ProductModel.Name,
                Description = ProductModel.Description,
                Price = ProductModel.Price,
                Stock = ProductModel.Stock,
                CategoryId = ProductModel.CategoryId,
                CreatedAt = ProductModel.CreatedAt
            };
        }
        public static Product ToFromCreateDOT(this CreateProductRequest Product)
        {
            return new Product
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Stock = Product.Stock,
                CategoryId = Product.CategoryId,
                CreatedAt = DateTime.Now
            };
        }
    }
}

