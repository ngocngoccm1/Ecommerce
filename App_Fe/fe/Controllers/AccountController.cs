using System.Text;
using App.DTO.Account;
using DTO.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            ViewBag.IsLogin = false;
            return View();
        }
        public IActionResult Login()
        {
            ViewBag.IsLogin = false;
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.IsLogin = false;
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
                    ViewBag.IsLogin = true;
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    // In ra phản hồi lỗi
                    return RedirectToAction("Create", "Products");

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
                    ViewBag.IsLogin = true;
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    // In ra phản hồi lỗi
                    return RedirectToAction("Create", "Products");

                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("token"); // Clear the token
            ViewBag.IsLogin = false;
            return RedirectToAction("Login");
        }



        //------------------------------------------------------------------------------------------------
    }
}
