using HKDR.DomainEntities.Entities.HR;
using HKDR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HKDR.Repository.Repository
{
    public  class EmployeeRepository : Repository<Employee>, HKDR.Repository.IRepository.IEmployeeRepository
    {
        public EmployeeRepository(ERPDbContext context): base(context){ }
             
        public async Task<int> CountActiveAsync()
        {
            return await _context.Employees.CountAsync(e => e.IsActive);
        }
        public async Task<Employee?> GetByIdWithDepartmentAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
