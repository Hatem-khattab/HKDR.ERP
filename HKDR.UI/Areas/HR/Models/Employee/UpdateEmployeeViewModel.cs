using System.ComponentModel.DataAnnotations;

namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class UpdateEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; }
    }
}
