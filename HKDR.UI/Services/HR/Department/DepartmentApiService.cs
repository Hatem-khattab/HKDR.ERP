using HKDR.Common.DTOs.HR.Department;
using HKDR.UI.Areas.HR.Models;
using HKDR.UI.Services.HR;
using HKDR.UI.Services.HR.Department;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class DepartmentApiService : IDepartmentApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepartmentApiService(HttpClient http, IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<DepartmentViewModel>> GetAllAsync()
    {
        // جيب التوكن من السيشن
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");

        // ضيفه بالهيدر
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.GetAsync("api/hr/departments");

        response.EnsureSuccessStatusCode();

        var dtoList = await response.Content.ReadFromJsonAsync<List<DepartmentDto>>();

        return dtoList!.Select(d => new DepartmentViewModel
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
    }
}
