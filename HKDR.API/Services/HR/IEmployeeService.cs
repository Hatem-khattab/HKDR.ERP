using HKDR.Common.DTOs.HR.EmployeesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.API.Services.HR
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateEmployeeDto dto);
        Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
