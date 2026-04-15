using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Common.DTOs.HR.EmployeesDto
{
    
        public class UpdateEmployeeDto
        {
            public string FullName { get; set; } = string.Empty;
            public decimal BasicSalary { get; set; }
            public int DepartmentId { get; set; }
            public bool IsActive { get; set; }
            public int Id { get; set; }


    }


}
