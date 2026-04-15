using HKDR.UI.Areas.HR.Models.Employee;

namespace HKDR.UI.Services.HR.Employee
{
    public interface IEmployeeApiService
    {
        Task<List<EmployeeViewModel>> GetAllAsync();
        Task<EmployeeViewModel?> GetByIdAsync(int id);

        Task CreateAsync(CreateEmployeeViewModel model);
        Task UpdateAsync(EditEmployeeViewModel model);

        Task DeleteAsync(int id);
    }
}
