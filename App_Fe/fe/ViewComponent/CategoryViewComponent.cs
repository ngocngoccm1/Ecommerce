using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class CategoryViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public CategoryViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5155/api/Category");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<CategoryModel>>(jsonData) ?? new List<CategoryModel>();

            return View(data);
        }

        return View(new List<CategoryModel>());
    }
}
