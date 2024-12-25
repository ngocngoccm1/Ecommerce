using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class CartViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public CartViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public IViewComponentResult Invoke(List<CartItemModel> cartModel)
    {

        // var response = await _httpClient.GetAsync($"http://localhost:5155/api/Product/{id}");
        // if (response.IsSuccessStatusCode)
        // {
        //     var jsonData = await response.Content.ReadAsStringAsync();
        //     var data = JsonConvert.DeserializeObject<ProductModel>(jsonData);

        //     return View(data);
        // }

        return View(cartModel);
    }

}
