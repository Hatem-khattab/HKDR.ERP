using System;


namespace HKDR.Common.DTOs.DashBoard
{

    
        public class PayrollTransaction
        {
            public int Id { get; set; }               // المفتاح الأساسي
            public int EmployeeId { get; set; }       // علاقة بالموظف
            public decimal GrossSalary { get; set; }
            public decimal NetSalary { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal LoanAmount { get; set; }
            public DateTime TransactionDate { get; set; }
        }
    

}
