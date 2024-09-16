using App.DTO.Product;
using App.Models;
using App.Helpers;
using OfficeOpenXml;
using App.Interface;

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
                Image = ProductModel.Image,
                CategoryName = ProductModel.Category.Name,
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
                Image = ImageHelper.EncodeImageToBase64(Product.Image),
                CreatedAt = DateTime.Now
            };
        }


    }
}

