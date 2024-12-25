using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using App.DTO.Account;
using DTO.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using Newtonsoft.Json;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5155/");
        }
        public IActionResult index()
        {
            var token = HttpContext.Session.GetString("token");
            var token_admin = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login"); // Redirect if token is not found
            }
            else
            {
                if (!string.IsNullOrEmpty(token_admin))
                {
                    return RedirectToAction("Admin"); // Redirect if token is not found
                }
                else
                {
                    return RedirectToAction("Index", "Products"); // Redirect if token is not found

                }
            }
        }
        public async Task<IActionResult> Review(int id, string content)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:5155/Review?id={id}&content={content}";

            // Tạo request và gửi đi
            var response = await _httpClient.PostAsync(url, null);

            // Kiểm tra trạng thái trả về
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Details", "Products", new { id = id });

            }
            return RedirectToAction("Details", "Products", new { id = id });
        }
        public async Task<IActionResult> like(string id, int product_id)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:5155/like?id={id}";

            // Tạo request và gửi đi
            var response = await _httpClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Details", "Products", new { id = product_id });
            }
            return RedirectToAction("Details", "Products", new { id = product_id });
        }

        public async Task<IActionResult> Admin()
        {
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response1 = await _httpClient.GetAsync("/manager");
            if (response1.IsSuccessStatusCode)
            {
                var jsonData = await response1.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<List<User_CartDto>>(jsonData) ?? new List<User_CartDto>();
                return View(user);
            }
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = "api/account";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(jsonData) ?? new UserModel();
                ViewBag.user = user;
                return View();

            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"api/account?id={id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
                return RedirectToAction("Admin");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Xóa người dùng thất bại: {errorMessage}";
                return RedirectToAction("Admin"); // Redirect to admin page if failure
            }


        }
        public async Task<IActionResult> Detail()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response1 = await _httpClient.GetAsync("/IsAdmin");
            if (response1.IsSuccessStatusCode)
            {
                return RedirectToAction("Admin");

            }
            else
            {
                var url = "api/account";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(jsonData) ?? new UserModel();
                    return View(user);
                }
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/account/login", content);

                // var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Đăng nhập thành công
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<NewUserDto>(data) ?? new NewUserDto();
                    HttpContext.Session.SetString("token", result.Token);

                    // Kiểm tra vai trò và chuyển hướng tương ứng
                    var token = HttpContext.Session.GetString("token");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response1 = await _httpClient.GetAsync("/IsAdmin");
                    if (response1.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token_admin", result.Token);
                        return RedirectToAction("Admin");

                    }
                    else
                        return RedirectToAction("Index", "Products");
                }
                else
                {
                    // In ra phản hồi lỗi
                    TempData["ErrorMessage"] = "Sai tên đăng nhập hoặc mật khẩu!";

                    return RedirectToAction("Login");

                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/account/register", content);

                // var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Đăng nhập thành công
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<NewUserDto>(data) ?? new NewUserDto();
                    HttpContext.Session.SetString("token", result.Token);
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    // In ra phản hồi lỗi
                    TempData["ErrorMessage"] = "Có lỗi không thể đăng kí!";

                    return RedirectToAction("Register");


                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("token"); // Clear the token
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<ActionResult> Edit([FromForm] UserUpdate userUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var content = new MultipartFormDataContent())
            {
                // Thêm các thuộc tính vào nội dung
                content.Add(new StringContent(userUpdate.Gender), "Gender");
                content.Add(new StringContent(userUpdate.FullName), "FullName");
                content.Add(new StringContent(userUpdate.MoreInfor), "MoreInfor");
                content.Add(new StringContent(userUpdate.Email), "Email");
                content.Add(new StringContent(userUpdate.PhoneNumber), "PhoneNumber");
                content.Add(new StringContent(userUpdate.Street), "Street");
                content.Add(new StringContent(userUpdate.City), "City");
                content.Add(new StringContent(userUpdate.State), "State");
                content.Add(new StringContent(userUpdate.ZipCode), "ZipCode");

                // Thêm hình ảnh vào nội dung
                // Add profile picture to the content if it exists
                if (userUpdate.ProfilePictureUrl != null && userUpdate.ProfilePictureUrl.Length > 0)
                {
                    var imageContent = new StreamContent(userUpdate.ProfilePictureUrl.OpenReadStream());
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(userUpdate.ProfilePictureUrl.ContentType);
                    content.Add(imageContent, "ProfilePictureUrl", userUpdate.ProfilePictureUrl.FileName);
                }



                var token = HttpContext.Session.GetString("token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync("/api/account/Edit", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Detail", "Account");

                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error updating account: {errorMessage}");
                }


            }
        }


        [HttpPost]
        public async Task<ActionResult> Payment(int orderId, int paymentId, string description, int amount)
        {

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account"); // Redirect if token is not found
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "/api/ZaloPay/create-order"; // Địa chỉ API

            // Tạo nội dung JSON request
            var createOrderRequest = new
            {
                OrderId = paymentId,
                Description = description,
                Amount = amount
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(createOrderRequest),
                Encoding.UTF8,
                "application/json-patch+json");


            // Gửi yêu cầu POST
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                // string orderUrl = JsonObject.RootElement.GetProperty("order_url").GetString();
                ZaloPayResponse content = JsonConvert.DeserializeObject<ZaloPayResponse>(jsonData) ?? new ZaloPayResponse();
                // string jsonContent1 = JsonConvert.SerializeObject(content);
                string order_url = content.order_url ?? "";
                int order_id = orderId;
                string order_tran_id = content.app_trans_id ?? string.Empty;
                if (order_url != "")
                {
                    TempData["SuccessMessage"] = "Thanh toán thành công";
                    var orderTransaction = new { ord_id = order_id, tran_id = order_tran_id };
                    // Serialize đối tượng thành JSON
                    string json = System.Text.Json.JsonSerializer.Serialize(orderTransaction);
                    string json1 = System.Text.Json.JsonSerializer.Serialize(order_id);
                    HttpContext.Session.SetString("OrderTransaction", json);
                    HttpContext.Session.SetString("order_id", json1);
                    return Redirect(order_url);
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi khi thanh toán";
                    return RedirectToAction("Order", "Index");
                }

            }
            return View();

        }

        //------------------------------------------------------------------------------------------------
    }
}
public class ZaloPayResponse
{
    public int return_code { get; set; } // "return_code"
    public string? return_message { get; set; } // "return_message"
    public int sub_return_code { get; set; } // "sub_return_code"
    public string? sub_return_message { get; set; } // "sub_return_message"
    public string? zp_trans_token { get; set; } // "zp_trans_token"
    public string? order_url { get; set; } // "order_url"
    public string? order_token { get; set; } // "order_token"
    public string? app_trans_id { get; set; } // "app_trans_id"
}
