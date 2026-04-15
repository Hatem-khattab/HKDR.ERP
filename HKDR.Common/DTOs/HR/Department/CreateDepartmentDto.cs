using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Common.DTOs.HR.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
