using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class navViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public navViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var response = await _httpClient.GetStringAsync($"http://localhost:5155/api/Category/{id}");

        if (response != null)
        {
            return View(JsonConvert.DeserializeObject<CategoryModel>(response) ?? new CategoryModel());
        }
        return View(new CategoryModel());
    }
}
