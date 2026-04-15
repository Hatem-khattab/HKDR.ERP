using HKDR.UI.Services.Auth;
using HKDR.UI.Services.HR.Dashboard;
using HKDR.UI.Services.HR.Department;
using HKDR.UI.Services.HR.Employee;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// Authentication (COOKIE) → UI فقط
// ===================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Auth/Login";
        options.AccessDeniedPath = "/Account/AccessDenied"; // صفحة رفض الوصول
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// ===================================================
// Session (JWT STORAGE)
// ===================================================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة الجلسة
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ===================================================
// Core Services
// ===================================================
builder.Services.AddHttpContextAccessor();

// ===================================================
// JWT Delegating Handler (يضيف JWT تلقائيًا)
// ===================================================
builder.Services.AddTransient<JwtHandler>();

// ===================================================
// HttpClient → API
// ===================================================
var apiBaseUrl = "https://localhost:7108/";

builder.Services.AddHttpClient<IEmployeeApiService, EmployeeApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpClient<IDepartmentApiService, DepartmentApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpClient<IHrDashboardApiService, HrDashboardApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtHandler>();

builder.Services.AddHttpClient<PayrollApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtHandler>();

// ===================================================
// MVC
// ===================================================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===================================================
// Pipeline
// ===================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ الترتيب مهم جدًا
app.UseSession();        // ✅ قبل Authorization
app.UseAuthentication(); // ✅ Cookie Auth
app.UseAuthorization();

// ===================================================
// Routing
// ===================================================
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
