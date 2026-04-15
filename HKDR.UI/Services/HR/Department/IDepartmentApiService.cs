using HKDR.UI.Areas.HR.Models;

namespace HKDR.UI.Services.HR.Department

{
    public interface IDepartmentApiService
    {
        Task<List<DepartmentViewModel>> GetAllAsync();
    }
}
