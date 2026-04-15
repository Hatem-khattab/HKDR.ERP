using HKDR.DomainEntities.Entities.HR;

public class PayrollTransaction
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }

    public bool IsClosed { get; set; } 
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal GrossSalary { get; set; }

    public decimal TaxAmount { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal OtherDeductions { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
