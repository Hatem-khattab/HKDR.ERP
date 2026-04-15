using HKDR.DomainEntities.Entities.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<int> CountActiveAsync();
        Task<Employee?> GetByIdWithDepartmentAsync(int id);
      

    }
}
