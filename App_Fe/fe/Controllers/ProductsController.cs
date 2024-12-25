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
        public IActionResult Test(string jsonData)
        {
            ZaloPayResponse content = JsonConvert.DeserializeObject<ZaloPayResponse>(jsonData) ?? new ZaloPayResponse();

            return View("Content", content);
        }

        public async Task<IActionResult> Index(int CategoryId, string? Name, int PageNumber, bool Isdescending)
        {
            var url = "api/Product?";
            ViewBag.CurrentPage = PageNumber == 0 ? 1 : PageNumber;
            ViewBag.CurrentCateroryID = CategoryId == 0 ? 2 : CategoryId;


            if (Name == null)
            {
                if (Isdescending.ToString() != null)
                {
                    url += $"CategoryId={ViewBag.CurrentCateroryID}&Isdescending={Isdescending}&PageNumber={ViewBag.CurrentPage}";
                }
                else
                {
                    url += $"CategoryId={ViewBag.CurrentCateroryID}&PageNumber={ViewBag.CurrentPage}";
                }
            }
            else
            {
                url += $"Name={Name}";
            }
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonData) ?? new List<ProductModel>();
                // Phương thức để lấy danh sách danh mục
                return View(products);
            }

            return View("Lỗi");
            // Hiển thị trang lỗi nếu không thành công
        }
        // Thêm vào ApiProductController
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var categories = await GetCategories(); // Phương thức để lấy danh sách danh mục
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(), // Giả sử Id là thuộc tính của CategoryModel
                Text = c.Name // Giả sử Name là thuộc tính của CategoryModel
            }); // Truyền danh sách danh mục vào view
            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            string apiUrl = $"api/Product/{id}";
            var token = HttpContext.Session.GetString("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Optionally log the error or show a more specific message.
                return RedirectToAction("Admin", "Account"); // You could create a more descriptive error view
            }

            return RedirectToAction("Index"); // Redirect to Index after successful deletion
        }


        public async Task<ActionResult> Details(int id)
        {

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
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Account", "Login"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var content = new MultipartFormDataContent())
            {
                // Thêm các thuộc tính vào nội dung
                content.Add(new StringContent(productRequest.Name), "Name");
                content.Add(new StringContent(productRequest.Description), "Description");
                content.Add(new StringContent(productRequest.Price.ToString()), "Price");
                content.Add(new StringContent(productRequest.Stock.ToString()), "Stock");
                content.Add(new StringContent(productRequest.CategoryId.ToString()), "CategoryId");

                // Thêm hình ảnh vào nội dung
                if (productRequest.Image != null && productRequest.Image.Length > 0)
                {
                    var imageContent = new StreamContent(productRequest.Image.OpenReadStream());
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(productRequest.Image.ContentType);
                    content.Add(imageContent, "image", productRequest.Image.FileName);
                }

                // Gửi yêu cầu POST đến API
                var response = await _httpClient.PostAsync("/api/Product", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Admin", "Acount");
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

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Vui lòng chọn một file để upload.";
                return RedirectToAction("Create", "Products");
            }
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Account", "Login"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var client = new HttpClient())
            {
                // client.BaseAddress = new Uri("http://localhost:5155"); // Đặt base URL của API

                using (var content = new MultipartFormDataContent())
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileStreamContent, "file", file.FileName);

                    var response = await _httpClient.PostAsync("/api/Product/import", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        ViewBag.Message = "Dữ liệu đã được import thành công!";
                        ViewBag.Products = result; // Gửi dữ liệu sản phẩm về view
                        return RedirectToAction("Admin", "Account");

                    }
                    else
                    {
                        ViewBag.Message = "Lỗi khi import dữ liệu: " + response.ReasonPhrase;
                    }
                }
            }
            return RedirectToAction("Create", "Products");

        }

    }
}