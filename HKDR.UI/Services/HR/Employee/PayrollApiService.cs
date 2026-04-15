using HKDR.Common.DTOs.HR.EmployeesDto;
using HKDR.UI.Areas.HR.Models.Employee;
using HKDR.UI.Services.HR.Employee;
using System.Net.Http.Headers;
using System.Text.Json;
public class PayrollApiService : IPayrollApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PayrollApiService(HttpClient http, IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
    }

    private void SetAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.User.FindFirst("JWT")?.Value;
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    // =========================
    // Get By Month
    // =========================
    public async Task<List<PayrollDto>> GetByMonthAsync(int year, int month)
    {
        SetAuthorizationHeader();

        var response = await _http.GetAsync($"api/hr/payroll?year={year}&month={month}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<PayrollDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    }

    // =========================
    // Get Current Month
    // =========================
    public async Task<List<PayrollDto>> GetCurrentMonthAsync()
    {
        var now = DateTime.Now;
        return await GetByMonthAsync(now.Year, now.Month);
    }

    // =========================
    // Generate Payslip
    // =========================
    public async Task<PayrollPayslipDto> GeneratePayslipAsync(int employeeId, DateTime month)
    {
        SetAuthorizationHeader();

        var response = await _http.PostAsync(
            $"api/hr/payroll/{employeeId}/generate?month={month:yyyy-MM-dd}", null);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PayrollPayslipDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    // =========================
    // Calculate Net Salary
    // =========================
    public async Task<decimal> CalculateNetSalaryAsync(int employeeId, DateTime month)
    {
        SetAuthorizationHeader();

        var response = await _http.GetAsync(
            $"api/hr/payroll/{employeeId}/net-salary?month={month:yyyy-MM-dd}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return decimal.Parse(content);
    }

    // =========================
    // Generate Payroll For All
    // =========================
    public async Task GeneratePayrollForAllAsync(DateTime month)
    {
        SetAuthorizationHeader();

        var response = await _http.PostAsync(
            $"api/hr/payroll/generate?month={month:yyyy-MM-dd}", null);

        response.EnsureSuccessStatusCode();
    }

    // =========================
    // Add Bonus
    // =========================
    public async Task AddBonusAsync(int employeeId, decimal amount, string reason)
    {
        SetAuthorizationHeader();

        var payload = new { EmployeeId = employeeId, Amount = amount, Reason = reason };
        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            System.Text.Encoding.UTF8,
            "application/json");

        var response = await _http.PostAsync("api/hr/payroll/add-bonus", content);
        response.EnsureSuccessStatusCode();
    }
}

