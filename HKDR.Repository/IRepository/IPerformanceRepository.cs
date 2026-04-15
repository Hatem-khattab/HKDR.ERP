using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.IRepository
{
    public interface IPerformanceRepository : IRepository<Performance>
    {
        Task<IEnumerable<Performance>> GetEmployeePerformanceAsync(int employeeId);
        Task AddTrainingAsync(int employeeId,string trainingPlan,string comments,DateTime evaluationDate);
        Task RecordKPIAsync(int employeeId,string kpi,int score,string comments,DateTime evaluationDate);
        Task ApplyPromotionAsync(int employeeId, string newPosition, decimal salaryIncrease);
        Task ApplyPerformanceBonusAsync(int employeeId, decimal amount, string reason);
    }
}
