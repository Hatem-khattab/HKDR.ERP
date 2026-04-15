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
    public class PayrollTransactionRepository
    : Repository<PayrollTransaction>, IPayrollTransactionRepository
    {
        private readonly ERPDbContext _context;

        public PayrollTransactionRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month)
        {
            return await _context.PayrollTransactions
                .Include(x => x.Employee)
                .Where(x => x.Year == year && x.Month == month)
                .ToListAsync();
        }

        public async Task<List<PayrollTransaction>> GetByEmployeeAsync(int employeeId, int year, int month)
        {
            return await _context.PayrollTransactions
                .Where(x =>
                    x.EmployeeId == employeeId &&
                    x.Year == year &&
                    x.Month == month)
                .ToListAsync();
        }
    }

}
