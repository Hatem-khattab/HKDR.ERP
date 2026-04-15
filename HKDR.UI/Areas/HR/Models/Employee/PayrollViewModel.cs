using System;
using System.Collections.Generic;

namespace HKDR.UI.Areas.HR.Models.Employee
{
    public class PayrollViewModel
    {
        public DateTime SelectedMonth { get; set; } = DateTime.Now;
        public List<PayrollDto> Payrolls { get; set; } = new();

    }
}
