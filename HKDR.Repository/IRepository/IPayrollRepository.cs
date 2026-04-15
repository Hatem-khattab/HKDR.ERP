using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.IRepository
{
    public interface IPayrollRepository: IRepository<PayrollTransaction>
    {
        Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month);
        Task<List<PayrollTransaction>> GetCurrentMonthAsync();
        Task AddRangeAsync(List<PayrollTransaction> payrolls);

        Task<decimal> CalculateNetSalaryAsync(int employeeId, DateTime month);
        Task<Payroll?> GeneratePayslipAsync(int employeeId, DateTime month);
        Task AddBonusAsync(int employeeId, decimal amount, string reason);
    }
}
