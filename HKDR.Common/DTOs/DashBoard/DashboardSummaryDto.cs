using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Common.DTOs.DashBoard
{
    public class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }

        public decimal GrossSalaryThisMonth { get; set; }
        public decimal NetSalaryThisMonth { get; set; }

        public decimal TotalTaxThisMonth { get; set; }
        public decimal TotalLoansThisMonth { get; set; }
    }

}
