using HKDR.Common.DTOs.HR.Department;

namespace HKDR.API.Services.HR
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllAsync();
        Task<int> CreateAsync(CreateDepartmentDto dto);
    }
}
