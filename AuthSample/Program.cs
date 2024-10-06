using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("Bearer").AddJwtBearer();
    // ���������� �������� ��������������
          // ����������� �������������� � ������� jwt-�������
builder.Services.AddAuthorization();            // ���������� �������� �����������

var app = builder.Build();

app.UseAuthentication();   // ���������� middleware �������������� 
app.UseAuthorization();   // ���������� middleware ����������� 

app.Map("/hello", [Authorize] () => "Hello World!");
app.Map("/", () => "Home Page");

app.Run();