using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using App.Controllers;
using App.Interface;
using App.DTO;
using App.Models;
using App.Helpers;
using App.DTO.Product;
using App.Mappers;
using Microsoft.AspNetCore.Http;

namespace App.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<IReviewRepository> _mockRepo1;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockRepo1 = new Mock<IReviewRepository>();
            _controller = new ProductController(_mockRepo.Object, _mockRepo1.Object);
        }

        [Fact]
        public async Task GetAllProduct_ReturnsOkResult()
        {
            // Arrange
            var products = new List<Product>
        {
        new Product
        {
            ProductId = 1,
            Name = "Product1",
            Description = "Description1",
            Price = 10.00m,
            Stock = 100,
            Image = "image1.jpg",
            Category = new Category { Name = "Category1" },  // Mock Category cho GetAll
            CategoryId = 1
        },
        new Product
        {
            ProductId = 2,
            Name = "Product2",
            Description = "Description2",
            Price = 20.00m,
            Stock = 200,
            Image = "image2.jpg",
            Category = new Category { Name = "Category2" },  // Mock Category cho GetAll
            CategoryId = 2
        }
        };

            _mockRepo.Setup(repo => repo.GetAllProductAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProduct();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);

            // Chuyển đổi IEnumerable<ProductDTO> thành List<ProductDTO>
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(okResult.Value).ToList();  // Chuyển thành List<ProductDTO>

            // Kiểm tra số lượng sản phẩm trả về
            Assert.Equal(2, returnProducts.Count);

            // Kiểm tra các thuộc tính của từng ProductDTO
            Assert.Equal("Product1", returnProducts[0].Name);
            Assert.Equal("Product2", returnProducts[1].Name);
            Assert.Equal("Category1", returnProducts[0].CategoryName);  // Kiểm tra CategoryName
            Assert.Equal("Category2", returnProducts[1].CategoryName);  // Kiểm tra CategoryName
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var productId = 3;
            var product = new Product
            {
                ProductId = productId,
                Name = "Product1",
                Description = "Description1",
                Price = 10.00m,
                Stock = 100,
                Image = "image1.jpg",
                CategoryId = 1,
                Category = new Category { Name = "Category1" },
                CreatedAt = DateTime.Now
            };


            _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);


            var returnProductDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(productId, returnProductDto.ProductId); // Kiểm tra ID sản phẩm
            Assert.Equal("Product1", returnProductDto.Name); // Kiểm tra tên sản phẩm
            Assert.Equal(10.00m, returnProductDto.Price); // Kiểm tra giá sản phẩm
            Assert.Equal("Category1", returnProductDto.CategoryName); // Kiểm tra CategoryName
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenImageIsNull()
        {
            // Arrange
            var createProductRequest = new CreateProductRequest
            {
                Name = "New Product",
                Description = "New product description",
                Price = 25.00m,
                Stock = 100,
                Image = null, // Không có ảnh gửi lên
                CategoryId = 1
            };

            // Act
            var result = await _controller.Create(createProductRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Hình ảnh không được gửi.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenProductIsCreated()
        {
            // Arrange
            var createProductRequest = new CreateProductRequest
            {
                Name = "New Product",
                Description = "New product description",
                Price = 25.00m,
                Stock = 100,
                Image = new FormFile(new MemoryStream(), 0, 0, "file", "image.jpg"), // Giả sử có ảnh gửi lên
                CategoryId = 1
            };

            Product productModel = createProductRequest.ToFromCreateDOT();

            // var productDto = new ProductDTO
            // {
            //     ProductId = productModel.ProductId,
            //     Name = productModel.Name,
            //     Description = productModel.Description,
            //     Price = productModel.Price,
            //     CategoryName = "Category1" // Giả sử có tên danh mục
            // };

            // Mock hành vi của repository để trả về sản phẩm mới được tạo
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(productModel);

            // Act
            var result = await _controller.Create(createProductRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Kiểm tra kết quả trả về có phải OkObjectResult không
            Assert.NotNull(okResult.Value); // Kiểm tra giá trị trả về không null

            // Kiểm tra rằng kết quả trả về là ProductDTO
            var returnProductDto = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productModel.ProductId, returnProductDto.ProductId); // Kiểm tra ID sản phẩm mới
            Assert.Equal("New Product", returnProductDto.Name); // Kiểm tra tên sản phẩm mới
            Assert.Equal(25.00m, returnProductDto.Price); // Kiểm tra giá sản phẩm mới
            Assert.Equal(1, returnProductDto.CategoryId); // Kiểm tra CategoryName của sản phẩm mới
        }



        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 999; // ID không tồn tại
            _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null); // Trả về null khi không tìm thấy sản phẩm

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result); // Kiểm tra xem kết quả có phải là NotFoundResult không
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenProductIsUpdatedSuccessfully()
        {
            // Arrange
            var productId = 1;
            var updateDto = new UpdateProductRequest { Name = "Updated Product", Price = 20.00m };

            var updatedProduct = new Product
            {
                ProductId = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 20.00m,
                Stock = 50,
                Image = "updated_image.jpg",
                CategoryId = 1,
                Category = new Category { Name = "Category1" },
                CreatedAt = DateTime.Now
            };

            _mockRepo.Setup(repo => repo.UpdateAsync(productId, updateDto))
                     .ReturnsAsync(updatedProduct);  // Simulate repository returning updated product

            // Act
            var result = await _controller.Update(productId, updateDto); // Call the controller method

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check if it returns OkObjectResult
            var returnProductDto = Assert.IsType<ProductDTO>(okResult.Value); // Assert the return type is ProductDTO

            Assert.Equal(productId, returnProductDto.ProductId); // Check if the returned product ID matches
            Assert.Equal("Updated Product", returnProductDto.Name); // Check if the name matches
            Assert.Equal(20.00m, returnProductDto.Price); // Check if the price matches
            Assert.Equal("Category1", returnProductDto.CategoryName); // Check if the category name matches
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductIsNotFound()
        {
            // Arrange
            var productId = 1;
            var updateDto = new UpdateProductRequest { Name = "Updated Product", Price = 20.00m };

            _mockRepo.Setup(repo => repo.UpdateAsync(productId, updateDto))
                     .ReturnsAsync((Product?)null);  // Simulate product not found (returns null)

            // Act
            var result = await _controller.Update(productId, updateDto); // Call the controller method

            // Assert
            Assert.IsType<NotFoundResult>(result); // Assert that the result is NotFoundResult
        }
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenProductIsDeletedSuccessfully()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                ProductId = productId,
                Name = "deleted Product",
                Description = "Deleted Description",
                Price = 20.00m,
                Stock = 50,
                Image = "Deleted_image.jpg",
                CategoryId = 1,
                Category = new Category { Name = "Category1" },
                CreatedAt = DateTime.Now
            };

            _mockRepo.Setup(repo => repo.DeledeAsync(productId)) // Mock the delete operation
                     .ReturnsAsync(product);  // Simulate successful deletion by returning the deleted product

            // Act
            var result = await _controller.Delete(productId);  // Call the controller action

            // Assert
            Assert.IsType<NoContentResult>(result);  // Assert that the result is NoContent (HTTP 204)
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductIsNotFound()
        {
            // Arrange
            var productId = 1;

            _mockRepo.Setup(repo => repo.DeledeAsync(productId))  // Mock the delete operation
                     .ReturnsAsync((Product?)null);  // Simulate that the product does not exist by returning null

            // Act
            var result = await _controller.Delete(productId);  // Call the controller action

            // Assert
            Assert.IsType<NotFoundResult>(result);  // Assert that the result is NotFound (HTTP 404)
        }

    }
}