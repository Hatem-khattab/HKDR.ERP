// إدارة الرواتب
using HKDR.DomainEntities.Entities.HR;
using System.ComponentModel.DataAnnotations;

public class Payroll
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal SocialSecurityEmployee { get; set; }
    public decimal SocialSecurityCompany { get; set; }
    public decimal IncomeTax { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime Month { get; set; }
}