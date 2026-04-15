using HKDR.Common.DTOs.HR;
using HKDR.Common.DTOs.HR.EmployeesDto;
using HKDR.UI.Areas.HR.Models.Employee;
using HKDR.UI.Services.HR.Employee;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

public class EmployeeApiService : IEmployeeApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeApiService(HttpClient http, IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
    }

    // ====================== Helper لإرسال JWT ======================
    private void SetAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.User.FindFirst("JWT")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // ====================== Create Employee ======================
    public async Task CreateAsync(CreateEmployeeViewModel model)
    {
        SetAuthorizationHeader();

        var dto = new CreateEmployeeDto
        {
            EmployeeNumber = model.EmployeeNumber,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DepartmentId = model.DepartmentId,
            JobTitle = model.JobTitle,
            BasicSalary = model.BasicSalary,
            IsActive = model.IsActive,
            HireDate = DateTime.UtcNow
        };

        var response = await _http.PostAsJsonAsync("api/hr/employees", dto);
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Failed to create employee");
    }

    // ====================== Get All Employees ======================
    public async Task<List<EmployeeViewModel>> GetAllAsync()
    {
        SetAuthorizationHeader();

        var response = await _http.GetAsync("api/hr/employees");

        // 👇 الحل هنا
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<EmployeeViewModel>();

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var dtos = JsonSerializer.Deserialize<List<EmployeeDto>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

        return dtos.Select(e => new EmployeeViewModel
        {
            Id = e.Id,
            FullName = e.FullName,
            JobTitle = e.JobTitle,
            DepartmentName = e.DepartmentName,
            BasicSalary = e.BasicSalary,
            IsActive = e.IsActive,
            DepartmentId = e.DepartmentId
        }).ToList();
    }

    // ====================== Update Employee ======================
    public async Task UpdateAsync(EditEmployeeViewModel model)
    {
        SetAuthorizationHeader();

        var dto = new UpdateEmployeeDto
        {
            Id = model.Id,
            FullName = model.FullName,
            DepartmentId = model.DepartmentId,
            BasicSalary = model.BasicSalary,
            IsActive = model.IsActive
        };

        var response = await _http.PutAsJsonAsync(
            $"api/hr/employees/{model.Id}",
            dto
        );

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Failed to update employee");
    }

    // ====================== Delete Employee ======================
    public async Task DeleteAsync(int id)
    {
        SetAuthorizationHeader();

        var response = await _http.DeleteAsync($"api/hr/employees/{id}");

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Failed to delete employee");
    }

    // ====================== Get Employee By Id ======================
    public async Task<EmployeeViewModel?> GetByIdAsync(int id)
    {
        SetAuthorizationHeader();

        var response = await _http.GetAsync($"api/hr/employees/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeViewModel>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
