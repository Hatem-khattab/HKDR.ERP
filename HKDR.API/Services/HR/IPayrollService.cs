using HKDR.DomainEntities.Entities.HR;

namespace HKDR.API.Services.HR
{
    public interface IPayrollService
    {
        Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month);
        Task<PayrollDTO> GeneratePayslipAsync(int employeeId, DateTime month);
        Task AddBonusAsync(int employeeId, decimal amount, string reason);
        Task GeneratePayrollForAllAsync(DateTime month);
        Task ClosePayrollMonthAsync(DateTime month);

    }

}
