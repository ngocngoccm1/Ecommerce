using System.Security.Claims;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
public static class HtmlHelpers
{
    public static IHtmlContent LoginStatus(this IHtmlHelper htmlHelper)
    {
        var httpContext = htmlHelper.ViewContext.HttpContext;

        // Check if HttpContext is null
        if (httpContext == null)
        {
            return new HtmlString("<p>Vui lòng đăng nhập.</p>");
        }

        // Retrieve the token from session
        var token = httpContext.Session.GetString("Token");

        // Check if token is null
        if (string.IsNullOrEmpty(token))
        {
            return new HtmlString("<p>Vui lòng đăng nhập.</p>");
        }

        // Create ClaimsPrincipal from the token (you'll need to decode it)


        // Check if the user is authenticated


        // Safely access the user's name
        var userName = "Người dùng"; // Fallback if Name is null

        return new HtmlString($"<p>Chào mừng, {userName}!</p>");
    }


}
