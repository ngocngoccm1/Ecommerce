using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using App.Controllers;
using App.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using App.DTO.Cart;
using App.Models;


namespace App.Tests.Controllers
{

    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly CartController _controller;
        private readonly ClaimsPrincipal _user;

        public CartControllerTests()
        {
            // Mock the ICartService
            _mockCartService = new Mock<ICartService>();

            // Set up an authenticated user (Mocking HttpContext.User)
            _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }, "TestAuthentication"));

            // Create the controller instance
            _controller = new CartController(_mockCartService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = _user // Set the mocked user for the controller
                    }
                }
            };
        }
        [Fact]
        public async Task GetCart_ReturnsOkResult_WhenUserIsAuthenticated()
        {
            // Arrange
            var userId = "test-user-id";
            var cart = new CartDto { Items = new List<CartItem>() };

            // Mock the GetCart method of the ICartService
            _mockCartService.Setup(service => service.GetCart(userId))
                            .ReturnsAsync(cart);

            // Act
            var result = await _controller.GetCart(); // Call the controller action

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCart = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(cart.Items.Count, returnCart.Items.Count);
        }

        [Fact]
        public async Task GetCart_WithId_ReturnsOkResult_WhenCartIsFound()
        {
            // Arrange
            var userId = "test-user-id";
            var cart = new CartDto { Items = new List<CartItem>() };

            // Mock the GetCart method of the ICartService
            _mockCartService.Setup(service => service.GetCart(userId))
                            .ReturnsAsync(cart);

            // Act
            var result = await _controller.GetCart(userId); // Call the controller action

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCart = Assert.IsType<CartDto>(okResult.Value);
            Assert.Equal(cart.Items.Count, returnCart.Items.Count);
        }
        [Fact]
        public async Task AddToCart_ReturnsOkResult()
        {
            // Arrange
            var cartItem = new CartItemDto
            {
                ProductId = 1,
                Quantity = 2
            };
            var userId = "test-user-id";

            // Mock the AddToCart method of the ICartService
            _mockCartService.Setup(service => service.AddToCart(cartItem, userId))
                            .Returns(Task.CompletedTask);  // Simulate successful addition

            // Act
            var result = await _controller.AddToCart(cartItem); // Call the controller action

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Assert that it's OkObjectResult
            Assert.Equal("Đã thêm vào giỏ hàng", okResult.Value); // Verify that the correct success message is returned
        }
        [Fact]
        public async Task RemoveFromCart_ReturnsOkResult_WhenItemIsRemovedSuccessfully()
        {
            // Arrange
            var productId = 1;

            // Mock the RemoveFromCart method of the ICartService
            _mockCartService.Setup(service => service.RemoveFromCart(productId))
                            .Returns(Task.CompletedTask); // Simulate successful removal

            // Act
            var result = await _controller.RemoveFromCart(productId); // Call the controller action

            // Assert
            var okResult = Assert.IsType<OkResult>(result); // Assert that the result is OkResult
        }


    }
}

