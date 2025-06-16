using AISDproject.Data;
using AISDproject.Repositeries.Implementation;
using AISDproject.Repositeries;
using AISDproject.Services.Implementation;
using AISDproject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // ✅ Thêm using này nếu cần

var builder = WebApplication.CreateBuilder(args);

// Đăng ký các services
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();

// Đăng ký DbContext
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký MVC
builder.Services.AddControllersWithViews();

// ✅ Cấu hình cookie authentication — quan trọng: thêm DefaultScheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MyCookieAuth"; // ✅ scheme mặc định
})
.AddCookie("MyCookieAuth", options =>
{
    options.LoginPath = "/Login/Index";
    options.LogoutPath = "/Logout/View/Shared/_StaffLayout";
    options.AccessDeniedPath = "/AccessDenied";
});

// Cấu hình session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// ✅ Quan trọng: authentication phải trước authorization
app.UseAuthentication();
app.UseAuthorization();

// Debug middleware (có thể bỏ sau khi test xong)
app.Use(async (context, next) =>
{
    Console.WriteLine($"Requested Path: {context.Request.Path}");
    await next();
});

// Cấu hình routing mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
