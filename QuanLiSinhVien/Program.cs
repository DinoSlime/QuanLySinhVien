using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data; // Thay bằng tên thư mục chứa class ApplicationDbContext của bạn

var builder = WebApplication.CreateBuilder(args);

// 1. ĐĂNG KÝ SQL SERVER VÀO HỆ THỐNG
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Cấu hình Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();