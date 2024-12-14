using Microsoft.EntityFrameworkCore;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ vào container
builder.Services.AddControllersWithViews();

// Thêm dịch vụ session
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ cache phân tán
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Hết hạn phiên sau 30 phút không hoạt động
    options.Cookie.HttpOnly = true; // Ngăn JavaScript truy cập cookie
    options.Cookie.IsEssential = true; // Làm cho cookie trở nên thiết yếu
});

var app = builder.Build();

// Cấu hình pipeline cho các yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Đảm bảo UseRouting được đặt trước UseAuthorization
app.UseRouting();

// Thêm middleware phân quyền sau khi định tuyến
app.UseAuthorization();

// Thêm middleware session trước UseEndpoints
app.UseSession();

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();