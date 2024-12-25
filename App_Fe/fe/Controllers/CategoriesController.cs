using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace App.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            var cate = await GetCategories();
            return View(cate);
        }
        public async Task<IActionResult> Details(int id)
        {
            var cate = await GetCategory(id);
            return View(cate);
        }
        //------------------------------------------------------------------------------------------------
        public async Task<CategoryModel> GetCategory(int id)
        {
            string apiUrl = $"http://localhost:5155/api/Category/{id}";
            var response = await _httpClient.GetStringAsync(apiUrl);

            if (response != null)
            {
                return JsonConvert.DeserializeObject<CategoryModel>(response) ?? new CategoryModel();
            }
            return new CategoryModel();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string cate)
        {
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string apiUrl = $"http://localhost:5155/api/Category/";
            var category = new { name = cate };
            var json = System.Text.Json.JsonSerializer.Serialize(category);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PostAsync(apiUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Category created successfully!";
                return RedirectToAction("Create", "Products");
            }
            return RedirectToAction("Create", "Products");

        }


        public async Task<List<CategoryModel>> GetCategories()
        {
            var url = "http://localhost:5155/api/Category"; // URL API danh mục
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CategoryModel>>(jsonData) ?? new List<CategoryModel>();
            }

            return new List<CategoryModel>(); // Trả về danh sách rỗng nếu có lỗi
        }
        public async Task<ActionResult> Delete(int id)
        {
            string apiUrl = $"http://localhost:5155/api/Category/{id}";
            var token = HttpContext.Session.GetString("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Optionally log the error or show a more specific message.
                return RedirectToAction("Index", "Categories"); // You could create a more descriptive error view
            }

            return RedirectToAction("Index"); // Redirect to Index after successful deletion
        }



    }
}
