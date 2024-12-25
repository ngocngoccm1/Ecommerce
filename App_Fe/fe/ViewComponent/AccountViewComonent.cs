using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class AccountViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public AccountViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        var token = HttpContext.Session.GetString("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"http://localhost:5155/api/Cart/id?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var cart = JsonConvert.DeserializeObject<CartModel>(jsonData) ?? new CartModel();
            return View(cart.Items);
        }
        return View(new List<CartItemModel>());
    }

}
