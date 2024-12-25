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

namespace App.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5155/");


        }

        [HttpPost]
        public async Task<IActionResult> Order(string ids)
        {
            List<int> idList = ids.Split(',').Select(int.Parse).ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = "api/Order";
            if (ModelState.IsValid)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(idList);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ThanhCong");
                }
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error : {errorMessage}");
            }

            // Log the response message for debugging

            return View("Error"); // Consider renaming "Lỗi" to "Error" for consistency
        }


        public IActionResult ThanhCong()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"api/Order/{id}";
            if (ModelState.IsValid)
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<OrderModel>(jsonData) ?? new OrderModel();
                    return View(order);
                }

            }
            return View("Error");
        }
        public async Task getStastus()
        {
            var payment_string = HttpContext.Session.GetString("OrderTransaction") ?? string.Empty;
            var orderTransaction = JsonConvert.DeserializeObject<OrderTransaction>(payment_string) ?? new OrderTransaction();
            string ord_id = orderTransaction.ord_id;
            string tran_id = orderTransaction.tran_id;
            var getstatus_url = $"/api/ZaloPay?apptransid={tran_id}&OrderId={ord_id}";
            await _httpClient.PostAsync(getstatus_url, null);

        }

        public IActionResult ThanhCong2(string url)
        {

            // Trả về view mà sẽ mở URL trong tab mới
            ViewBag.Url = url;
            return View();
        }
        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await getStastus();
            var token = HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = "api/Order/ALL";
            if (ModelState.IsValid)
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<OrderModel>>(jsonData) ?? new List<OrderModel>();
                    return View(orders);
                }

            }
            return View("Error");

        }

        // Thêm vào ApiProductController
    }
    public class OrderTransaction
    {
        public string ord_id { get; set; } = string.Empty;
        public string tran_id { get; set; } = string.Empty;

    }

}