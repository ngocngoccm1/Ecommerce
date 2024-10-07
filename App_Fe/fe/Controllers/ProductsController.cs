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

namespace App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5155/");


        }
        public IActionResult Test()
        {
            return View();
        }

        public async Task<IActionResult> Index(string? name)
        {
            if (IsLogin()) { ViewBag.IsLogin = true; }
            else { ViewBag.IsLogin = false; }
            var url = "api/Product";
            if (!string.IsNullOrEmpty(name))
            {
                url += $"?Name={name}";
            }
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonData);
                var categories = await GetCategories(); // Phương thức để lấy danh sách danh mục
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(), // Giả sử Id là thuộc tính của CategoryModel
                    Text = c.Name // Giả sử Name là thuộc tính của CategoryModel
                }); // Truyền danh sách danh mục vào view
                return View(products);
            }

            return View("Lỗi");
            // Hiển thị trang lỗi nếu không thành công
        }
        // Thêm vào ApiProductController
        public async Task<IActionResult> Create()
        {
            ViewBag.IsLogin = true;
            var categories = await GetCategories(); // Phương thức để lấy danh sách danh mục
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(), // Giả sử Id là thuộc tính của CategoryModel
                Text = c.Name // Giả sử Name là thuộc tính của CategoryModel
            }); // Truyền danh sách danh mục vào view
            return View();
        }

        public async Task<ActionResult> Details(int id)
        {
            if (IsLogin()) { ViewBag.IsLogin = true; }
            else { ViewBag.IsLogin = false; }
            string apiUrl = $"api/Product/{id}";
            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var product = JsonConvert.DeserializeObject<ProductModel>(response);
                if (product == null)
                {
                    return View("lỗi");
                }
                return View(product);
            }
            catch (HttpRequestException)
            {
                // Xử lý lỗi khi không thể gọi API
                return View("Error"); // Hoặc một view lỗi nào đó
            }

        }
        // Xử lý tạo sản phẩm
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var content = new MultipartFormDataContent())
            {
                // Thêm các thuộc tính vào nội dung
                content.Add(new StringContent(productRequest.Name), "name");
                content.Add(new StringContent(productRequest.Description), "description");
                content.Add(new StringContent(productRequest.Price.ToString()), "price");
                content.Add(new StringContent(productRequest.Stock.ToString()), "stock");
                content.Add(new StringContent(productRequest.CategoryId.ToString()), "categoryId");

                // Thêm hình ảnh vào nội dung
                if (productRequest.Image != null && productRequest.Image.Length > 0)
                {
                    var imageContent = new StreamContent(productRequest.Image.OpenReadStream());
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(productRequest.Image.ContentType);
                    content.Add(imageContent, "image", productRequest.Image.FileName);
                }

                // Gửi yêu cầu POST đến API
                var response = await _httpClient.PostAsync("http://localhost:5155/api/Product", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return StatusCode((int)response.StatusCode, "Có lỗi xảy ra khi tạo sản phẩm.");
            }
        }

        //----------------------------------------------------------------------------------------

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


        private bool IsLogin()
        {
            // Check if the token exists in the session
            var token = HttpContext.Session.GetString("token");
            return !string.IsNullOrEmpty(token); // Return true if the token is not null or empty
        }

    }
}