using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.IRepository
{
    public interface IPayrollTransactionRepository : IRepository<PayrollTransaction>
    {
        Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month);
        Task<List<PayrollTransaction>> GetByEmployeeAsync(int employeeId, int year, int month);
    }

}
