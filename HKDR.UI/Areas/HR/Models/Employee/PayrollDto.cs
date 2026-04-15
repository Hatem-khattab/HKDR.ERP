using System;
using System.ComponentModel.DataAnnotations;

namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class PayrollDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public decimal Allowances { get; set; }

        public decimal Deductions { get; set; }

        public decimal SocialSecurityEmployee { get; set; }

        public decimal SocialSecurityCompany { get; set; }

        public decimal IncomeTax { get; set; }

        [Required]
        public decimal NetSalary { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
