using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class Product_AdminViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public Product_AdminViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5155/api/Product/getAll");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ProductModel>>(jsonData);

            return View(data);
        }
        return View(new List<ProductModel>());
    }
}
