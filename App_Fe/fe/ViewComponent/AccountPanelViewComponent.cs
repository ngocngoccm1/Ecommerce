using System.Net.Http.Headers;
using DTO.Account;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

public class AccountPanelViewComponent : ViewComponent
{
    private readonly HttpClient _httpClient;

    public AccountPanelViewComponent(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public IViewComponentResult Invoke(UserModel u)
    {


        return View(u);
    }
}