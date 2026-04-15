namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
    }
}
