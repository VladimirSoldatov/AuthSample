using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/login", async (HttpContext context) =>
{
    var claims = new List<Claim>
    {
        new Claim (ClaimTypes.Name, "Tom"),
        new Claim ("languages", "English"),
        new Claim ("languages", "German"),
        new Claim ("languages", "Spanish")
    };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(claimsPrincipal);
    return Results.Redirect("/");
});
app.Map("/", (HttpContext context) =>
{
    var username = context.User.FindFirst(ClaimTypes.Name);
    var languages = context.User.FindAll("languages");
    // объединяем список claims в строку
    var languagesToString = "";
    foreach (var l in languages)
        languagesToString = $"{languagesToString} {l.Value}";
    return $"Name: {username?.Value}\nLanguages: {languagesToString}";
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "Данные удалены";
});

app.Run();