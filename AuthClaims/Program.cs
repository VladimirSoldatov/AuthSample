using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/login/{username}", async (string username, HttpContext context) =>
{
    var claims = new List<Claim> { new(ClaimTypes.Name, username) };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(claimsPrincipal);
    return $"Установлено имя {username}";
});
app.Map("/", (HttpContext context) =>
{
    var user = context.User.Identity;
    if (user is not null && user.IsAuthenticated)
        return $"UserName: {user.Name}";
    else return "Пользователь не аутентифицирован.";
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "Данные удалены";
});

app.Run();