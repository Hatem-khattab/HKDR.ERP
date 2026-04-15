using HKDR.Infrastructure.Data;
using HKDR.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.Repository
{
    public class PerformanceRepository : Repository<Performance>, IPerformanceRepository
    {
        public PerformanceRepository(ERPDbContext context) : base(context) { }

        public async Task<IEnumerable<Performance>> GetEmployeePerformanceAsync(int employeeId)
        {
            return await _context.Performances
                .Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddTrainingAsync(int employeeId,string trainingPlan,string comments,DateTime evaluationDate)
        {
            var training = new Performance
            {
                EmployeeId = employeeId,
                TrainingPlan = trainingPlan,
                Comments = comments,
                EvaluationDate = evaluationDate
            };

            await AddAsync(training);
            await SaveAsync();
        }


        public async Task RecordKPIAsync(int employeeId,string kpi,int score,string comments,DateTime evaluationDate)
        {
            var performance = new Performance
            {
                EmployeeId = employeeId,
                KPI = kpi,
                Score = score,
                Comments = comments,
                EvaluationDate = evaluationDate
            };

            await AddAsync(performance);
            await SaveAsync();
        }


        public async Task ApplyPromotionAsync(int employeeId, string JobTitle, decimal salaryIncrease)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee != null)
            {
                employee.JobTitle = JobTitle;
                employee.BasicSalary += salaryIncrease;
                _context.Employees.Update(employee);
                await SaveAsync();
            }
        }

        public async Task ApplyPerformanceBonusAsync(int employeeId, decimal amount, string reason)
        {
            var payroll = await _context.Payrolls.FirstOrDefaultAsync(p => p.EmployeeId == employeeId);
            if (payroll != null)
            {
                payroll.Allowances += amount;
                _context.Payrolls.Update(payroll);
                await SaveAsync();
            }
        }
    }

}

