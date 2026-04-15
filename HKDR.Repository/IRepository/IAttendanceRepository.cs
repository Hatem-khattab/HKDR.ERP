using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.IRepository
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task MarkAttendanceAsync(int employeeId, DateTime date, TimeSpan? checkIn, TimeSpan? checkOut);
        Task<IEnumerable<LeaveRequest>> GetLeavesAsync(int employeeId);
        Task<decimal> GetRemainingLeaveDaysAsync(int employeeId);
        Task LinkAbsenceToPayrollAsync(int employeeId, DateTime month);
    }
}
