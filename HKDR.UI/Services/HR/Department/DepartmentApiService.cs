using HKDR.Common.DTOs.HR.Department;
using HKDR.UI.Areas.HR.Models;
using HKDR.UI.Services.HR;
using HKDR.UI.Services.HR.Department;
using System.Net.Http.Json;

public class DepartmentApiService : IDepartmentApiService
{
    private readonly HttpClient _http;

    public DepartmentApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DepartmentViewModel>> GetAllAsync()
    {
        var dtoList = await _http.GetFromJsonAsync<List<DepartmentDto>>(
            "api/hr/departments");

        return dtoList!.Select(d => new DepartmentViewModel
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
    }
}
