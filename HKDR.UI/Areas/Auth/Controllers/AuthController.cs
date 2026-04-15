using HKDR.Common.DTOs.Auth;
using HKDR.UI.Areas.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Area("Auth")]
public class AuthController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7108/");
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        Console.WriteLine("Email: " + model.Email);
        Console.WriteLine("Password: " + model.Password);
        Console.WriteLine("🔥 LOGIN CONTROLLER HIT");
        if (!ModelState.IsValid)
            return View(model);

        var dto = new LoginDto { Email = model.Email, Password = model.Password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API ERROR: " + error);

            ModelState.AddModelError("", error); // 🔥 اعرض الحقيقي
            return View(model);
        }
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>(options);

        if (result == null || string.IsNullOrEmpty(result.AccessToken))
        {
            ModelState.AddModelError("", "Login failed");
            return View(model);
        }

        // ✅ خزّن JWT و RefreshToken في Session
        HttpContext.Session.SetString("JWT", result.AccessToken);
        HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

        // ✅ Cookie Auth للـ UI
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.Email),
        new Claim("CompanyId", result.CompanyId.ToString())
    };

        if (result.Roles != null && result.Roles.Any())
        {
            claims.Add(new Claim(ClaimTypes.Role, result.Roles.First()));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        Console.WriteLine("LOGIN SUCCESS");
        Console.WriteLine("TOKEN: " + result.AccessToken);
        Console.WriteLine("ROLE: " + result.Roles?.FirstOrDefault());
        var errorText = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API RESPONSE: " + errorText);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToAction("Index", "SystemSelection", new { area = "Pages" });

    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth", new { area = "Auth" });
    }
}
