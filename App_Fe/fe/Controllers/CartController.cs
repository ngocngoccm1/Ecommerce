using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Models;
using System.Text;
using DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using App.DTO.Cart;
using Microsoft.AspNetCore.Http;

namespace App.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5155/");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CartItemDto cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }

            using (var content = new MultipartFormDataContent())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var url = "api/Cart/add";

                content.Add(new StringContent(cartItem.ProductId.ToString()), "ProductId");
                content.Add(new StringContent(cartItem.Quantity.ToString()), "Quantity");

                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                // Log the response message for debugging
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error adding item to cart: {errorMessage}");
            }

            return View("Error"); // Consider renaming "Lỗi" to "Error" for consistency
        }



        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"/api/Cart/remove/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                // Optionally, you can add a success message or redirect
                TempData["SuccessMessage"] = "Item deleted successfully.";
                return RedirectToAction("Index"); // Redirect to the index or another action
            }
            else
            {
                // Handle the error case
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error deleting item: {errorMessage}");
                return View("Error"); // Redirect to an error view or show an error message
            }
        }

        public CartModel CART => HttpContext.Session.Get<CartModel>("CART") ?? new CartModel();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = "api/Cart";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var cart = JsonConvert.DeserializeObject<CartModel>(jsonData) ?? new CartModel();
                return View(cart.Items);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log errorContent for debugging
                return View(errorContent);
            }

        }

        // Thêm vào ApiProductController
    }
}