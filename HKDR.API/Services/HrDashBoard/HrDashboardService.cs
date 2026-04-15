using System.Collections.Generic;
using HKDR.Common.DTOs.DashBoard;
using HKDR.Repository.IRepository;

namespace HKDR.API.Services.HrDashBoard
{

    public class HrDashboardService : IHrDashboardService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IPayrollRepository _payrollRepo;

        public HrDashboardService(
            IEmployeeRepository employeeRepo,
            IPayrollRepository payrollRepo)
        {
            _employeeRepo = employeeRepo;
            _payrollRepo = payrollRepo;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var totalEmployees = await _employeeRepo.CountActiveAsync();

            var payroll = await _payrollRepo.GetCurrentMonthAsync()
                          ?? new List<PayrollTransaction>();

            return new DashboardSummaryDto
            {
                TotalEmployees = totalEmployees,
                GrossSalaryThisMonth = payroll.Sum(p => p.GrossSalary),
                NetSalaryThisMonth = payroll.Sum(p => p.NetSalary),
                TotalTaxThisMonth = payroll.Sum(p => p.TaxAmount),
                TotalLoansThisMonth = payroll.Sum(p => p.LoanAmount)
            };
        }
    }

}
