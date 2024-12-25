using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class ProductViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public ProductViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IViewComponentResult Invoke(List<ProductModel> products)
    {
        return View(products);
    }
}
