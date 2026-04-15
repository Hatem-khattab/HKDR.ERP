using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Common.DTOs.HR.EmployeesDto
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Job Info
        public int DepartmentId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public decimal BasicSalary { get; set; }


        // Status
        public bool IsActive { get; set; } = true;

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
