using HKDR.Infrastructure.Data;
using HKDR.Repository;
using HKDR.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

public class PayrollRepository: Repository<PayrollTransaction>, IPayrollRepository
{
    private readonly ERPDbContext _context;

    public PayrollRepository(ERPDbContext context)
              : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month)
    {
        return await _context.PayrollTransactions
            .Include(p => p.Employee)
            .Where(p => p.Year == year && p.Month == month)
            .ToListAsync();
    }

    public async Task<List<PayrollTransaction>> GetCurrentMonthAsync()
    {
        var now = DateTime.Now;
        return await GetByMonthAsync(now.Year, now.Month);
    }
    public async Task AddRangeAsync(List<PayrollTransaction> payrolls)
    {
        if (payrolls == null || !payrolls.Any())
            throw new ArgumentException("Payroll list is empty", nameof(payrolls));

        await _context.PayrollTransactions.AddRangeAsync(payrolls);
        await _context.SaveChangesAsync();
    }
    public async Task<decimal> CalculateNetSalaryAsync(int employeeId, DateTime month)
    {
        var payrolls = await _context.PayrollTransactions
            .Where(p => p.EmployeeId == employeeId && p.Year == month.Year && p.Month == month.Month)
            .ToListAsync();

        decimal totalEarnings = payrolls.Sum(p => p.GrossSalary); // نفترض عندك GrossAmount
        decimal totalDeductions = payrolls.Sum(p => p.OtherDeductions); // نفترض عندك Deductions

        return totalEarnings - totalDeductions;
    }
    public async Task<Payroll?> GeneratePayslipAsync(int employeeId, DateTime month)
    {
        var netSalary = await CalculateNetSalaryAsync(employeeId, month);
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null) return null;

        var payslip = new Payroll
        {
            EmployeeId = employeeId,
            Month = month,
            NetSalary = netSalary,
        };

        _context.Payrolls.Add(payslip);
        await _context.SaveChangesAsync();
        return payslip;
    }
    public async Task AddBonusAsync(int employeeId, decimal amount, string reason)
    {
        if (amount <= 0)
            throw new ArgumentException("Bonus amount must be greater than zero", nameof(amount));

        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            throw new InvalidOperationException("Employee not found");

        var bonusTransaction = new PayrollTransaction
        {
            EmployeeId = employeeId,
            Employee = employee,

            Year = DateTime.Now.Year,
            Month = DateTime.Now.Month,

            BasicSalary = 0,          // لأن هذا مجرد بونص
            Allowances = 0,
            GrossSalary = amount,     // البونص يضاف للـGrossSalary
            TaxAmount = 0,            // إذا عندك خصم ضريبة للبونص يمكن تعدّل هنا
            LoanAmount = 0,
            OtherDeductions = 0,
            NetSalary = amount,       // البونص صافي هنا

            CreatedAt = DateTime.UtcNow
        };

        _context.PayrollTransactions.Add(bonusTransaction);
        await _context.SaveChangesAsync();
    }







}
