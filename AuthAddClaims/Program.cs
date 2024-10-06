using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseAuthentication();
// ���������� ��������
app.MapGet("/addage", async (HttpContext context) =>
{
    if (context.User.Identity is ClaimsIdentity claimsIdentity)
    {
        claimsIdentity.AddClaim(new Claim("age", "37"));
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await context.SignInAsync(claimsPrincipal);
    }
    return Results.Redirect("/");
});
// �������� ��������
app.MapGet("/removephone", async (HttpContext context) =>
{
    if (context.User.Identity is ClaimsIdentity claimsIdentity)
    {
        var phoneClaim = claimsIdentity.FindFirst(ClaimTypes.MobilePhone);
        // ���� claim ������� ������
        if (claimsIdentity.TryRemoveClaim(phoneClaim))
        {
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal);
        }
    }
    return Results.Redirect("/");
});
app.MapGet("/login", async (HttpContext context) =>
{
    var username = "Tom";
    var company = "Microsoft";
    var phone = "+12345678901";

    var claims = new List<Claim>
    {
        new Claim (ClaimTypes.Name, username),
        new Claim ("company", company),
        new Claim(ClaimTypes.MobilePhone,phone)
    };
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    await context.SignInAsync(claimsPrincipal);
    return Results.Redirect("/");
});
app.Map("/", (HttpContext context) =>
{
    var username = context.User.FindFirst(ClaimTypes.Name);
    var phone = context.User.FindFirst(ClaimTypes.MobilePhone);
    var company = context.User.FindFirst("company");
    var age = context.User.FindFirst("age");
    return $"Name: {username?.Value}\nPhone: {phone?.Value}\n" +
    $"Company: {company?.Value}\nAge: {age?.Value}";
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "������ �������";
});

app.Run();