using HKDR.DomainEntities.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.DomainEntities.Entities.HR
{
    public class Employee : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string EmployeeNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Job Info
        public int CompanyId { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public string JobTitle { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public decimal BasicSalary { get; set; }

        // Status
        public bool IsActive { get; set; } = true;

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<LeaveRequest> Leaves { get; set; }
        public ICollection<Performance> Performances { get; set; }
    }
}