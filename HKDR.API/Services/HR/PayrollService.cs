using HKDR.DomainEntities.Entities.HR;
using HKDR.Repository.IRepository;
using HKDR.Repository.Repository;

namespace HKDR.API.Services.HR
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollTransactionRepository _repo;
        private readonly IEmployeeRepository _employeeRepo;

        public PayrollService(
            IPayrollTransactionRepository repo,
            IEmployeeRepository employeeRepo)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
        }

        public async Task<List<PayrollTransaction>> GetByMonthAsync(int year, int month)
        {
            return await _repo.GetByMonthAsync(year, month);
        }

        public async Task<PayrollDTO> GeneratePayslipAsync(int employeeId, DateTime month)
        {
            int year = month.Year;
            int monthNumber = month.Month;

            // 1️⃣ جلب معاملات الراتب للموظف بالشهر
            var transactions =
                await _repo.GetByEmployeeAsync(employeeId, year, monthNumber);

            if (!transactions.Any())
                throw new Exception("No payroll transactions found for this employee in this month");

            // 2️⃣ جلب بيانات الموظف
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            // 3️⃣ حساب القيم (غالبًا سجل واحد، بس خليها safe)
            var basicSalary = transactions.Sum(x => x.BasicSalary);
            var allowances = transactions.Sum(x => x.Allowances);
            var deductions = transactions.Sum(x =>
                x.TaxAmount +
                x.LoanAmount +
                x.OtherDeductions
            );

            var netSalary = transactions.Sum(x => x.NetSalary);

            // 4️⃣ بناء payslip DTO
            return new PayrollDTO
            {
                EmployeeId = employeeId,
                EmployeeName = employee.FullName,
                BasicSalary = basicSalary,
                Allowances = allowances,
                Deductions = deductions,
                NetSalary = netSalary,
                Month = new DateTime(year, monthNumber, 1)
            };
        }

        public async Task ClosePayrollMonthAsync(DateTime month)
        {
            int year = month.Year;
            int monthNumber = month.Month;

            // 1️⃣ جلب كل رواتب الشهر
            var transactions =
                await _repo.GetByMonthAsync(year, monthNumber);

            if (!transactions.Any())
                throw new Exception("No payroll records found for this month");

            // 2️⃣ التأكد إن الشهر مش مغلق
            if (transactions.All(x => x.IsClosed))
                throw new Exception("Payroll month is already closed");

            // 3️⃣ إغلاق الرواتب
            foreach (var tx in transactions)
            {
                tx.IsClosed = true;
            }

            await _repo.SaveAsync();
        }

        public async Task AddBonusAsync(int employeeId, decimal amount, string reason)
        {
            var tx = new PayrollTransaction
            {
                EmployeeId = employeeId,
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,

                GrossSalary = amount,
                CreatedAt = DateTime.UtcNow
            };

            // ✅ حساب الصافي
            PayrollCalculator.Calculate(tx);

            await _repo.AddAsync(tx);
            await _repo.SaveAsync();
        }

        public async Task GeneratePayrollForAllAsync(DateTime month)
        {
            var employees = await _employeeRepo.GetAllAsync();

            foreach (var emp in employees)
            {
                var exists = await _repo.GetByEmployeeAsync(
                    emp.Id, month.Year, month.Month);

                if (exists.Any())
                    continue;

                var tx = new PayrollTransaction
                {
                    EmployeeId = emp.Id,
                    Year = month.Year,
                    Month = month.Month,

                    BasicSalary = emp.BasicSalary,
                    GrossSalary = emp.BasicSalary,

                    CreatedAt = DateTime.UtcNow
                };

                // ✅✅ هنا الميثود اللي سألت عنها
                PayrollCalculator.Calculate(tx);

                await _repo.AddAsync(tx);
            }

            await _repo.SaveAsync();
        }


    }


}
