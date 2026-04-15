using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Common.DTOs.HR.EmployeesDto
{

    public class AddBonusDto
    {
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
