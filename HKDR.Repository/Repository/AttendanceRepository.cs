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
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ERPDbContext context) : base(context) { }

        public async Task MarkAttendanceAsync(int employeeId, DateTime date, TimeSpan? checkIn, TimeSpan? checkOut)
        {
            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = date,
                CheckIn = checkIn.HasValue ? (DateTime?)DateTime.Today.Add(checkIn.Value) : null,
                CheckOut = checkOut.HasValue ? (DateTime?)DateTime.Today.Add(checkOut.Value) : null
            };
            await AddAsync(attendance);
            await SaveAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeavesAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<decimal> GetRemainingLeaveDaysAsync(int employeeId)
        {
            var usedLeaves = await _context.LeaveRequests
                .Where(l => l.EmployeeId == employeeId && l.Status == "Approved")
                .SumAsync(l => l.Days);
            const decimal totalLeaves = 30;
            return totalLeaves - usedLeaves;
        }

        public async Task LinkAbsenceToPayrollAsync(int employeeId, DateTime month)
        {
            // مثال: ربط التأخير أو الغياب بالرواتب
            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Month == month);

            if (payroll != null)
            {
                // عدد أيام الغياب في الشهر
                var totalAbsentDays = await _context.Attendances
                    .Where(a => a.EmployeeId == employeeId && a.Date.Month == month.Month && a.Date.Year == month.Year)
                    .CountAsync(a => a.IsAbsent);

                // خصم 50 لكل يوم غياب (كمثال)
                payroll.Deductions += totalAbsentDays * 50;

                _context.Payrolls.Update(payroll);
                await _context.SaveChangesAsync(); // هنا SaveChangesAsync وليس SaveAsync
            }
        }
    }
}
