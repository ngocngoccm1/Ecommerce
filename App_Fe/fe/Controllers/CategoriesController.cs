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
            var cates = await GetCategories();
            return View(cates);
        }
        public async Task<IActionResult> Details(int id)
        {
            var cate = await GetCategory(id);
            return View(cate);
        }
        //------------------------------------------------------------------------------------------------
        private async Task<CategoryModel> GetCategory(int id)
        {
            string apiUrl = $"http://localhost:5155/api/Category/{id}";
            var response = await _httpClient.GetStringAsync(apiUrl);

            if (response != null)
            {
                return JsonConvert.DeserializeObject<CategoryModel>(response) ?? new CategoryModel();
            }
            return new CategoryModel();
        }
        private async Task<List<CategoryModel>> GetCategories()
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


    }
}
