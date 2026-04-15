using HKDR.Infrastructure.Data;

namespace HKDR.Repository.Repository
{
    public class DepartmentRepository : Repository<HKDR.DomainEntities.Entities.Department>, HKDR.Repository.IRepository.IDepartmentRepository
    {

        public DepartmentRepository(ERPDbContext context)
           : base(context)
        {
        }




    }
}
