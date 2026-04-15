using Microsoft.AspNetCore.Mvc.Rendering;

namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int DepartmentId { get; set; }

        public List<SelectListItem> Departments { get; set; } = new();

        public string JobTitle { get; set; } = null!;

        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; }
    }
}
