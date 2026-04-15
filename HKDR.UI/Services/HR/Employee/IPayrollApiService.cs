using HKDR.Common.DTOs.HR;
using HKDR.Common.DTOs.HR.EmployeesDto;
using HKDR.UI.Areas.HR.Models.Employee;

namespace HKDR.UI.Services.HR.Employee
{
    public interface IPayrollApiService
    {
        Task<List<PayrollDto>> GetByMonthAsync(int year, int month);
        Task<List<PayrollDto>> GetCurrentMonthAsync();
        Task<PayrollPayslipDto> GeneratePayslipAsync(int employeeId, DateTime month);
        Task AddBonusAsync(int employeeId, decimal amount, string reason);
        Task<decimal> CalculateNetSalaryAsync(int employeeId, DateTime month);
        Task GeneratePayrollForAllAsync(DateTime month);
    }
}