using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public List<SelectListItem> Departments { get; set; } = new();

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
