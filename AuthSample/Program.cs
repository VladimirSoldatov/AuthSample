using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("Bearer").AddJwtBearer();
    // добавление сервисов аутентификации
          // подключение аутентификации с помощью jwt-токенов
builder.Services.AddAuthorization();            // добавление сервисов авторизации

var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.Map("/hello", [Authorize] () => "Hello World!");
app.Map("/", () => "Home Page");

app.Run();