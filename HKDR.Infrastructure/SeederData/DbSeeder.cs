using HKDR.DomainEntities.Entities;
using HKDR.DomainEntities.Entities.Core;
using HKDR.DomainEntities.Entities.HR;
using HKDR.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HKDR.Infrastructure.SeederData
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ERPDbContext>();

            // 👇 تأكد من وجود قاعدة البيانات
            await context.Database.MigrateAsync();

            // =========================
            // Departments
            // =========================
            if (!await context.Departments.AnyAsync())
            {
                context.Departments.AddRange(
                    new Department { Name = "HR" },
                    new Department { Name = "IT" },
                    new Department { Name = "Finance" }
                );
                await context.SaveChangesAsync();
            }

            // =========================
            // Employees
            // =========================
            if (!await context.Employees.AnyAsync())
            {
                var departments = await context.Departments.ToListAsync();

                context.Employees.AddRange(
                    new Employee
                    {
                        EmployeeNumber = "EMP001",
                        FullName = "Ali Ahmad",
                        DepartmentId = departments.First(d => d.Name == "HR").Id,
                        JobTitle = "HR Manager",
                        BasicSalary = 1500,
                        IsActive = true,
                        HireDate = DateTime.UtcNow.AddMonths(-12),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Employee
                    {
                        EmployeeNumber = "EMP002",
                        FullName = "Sara Khalid",
                        DepartmentId = departments.First(d => d.Name == "IT").Id,
                        JobTitle = "Developer",
                        BasicSalary = 1200,
                        IsActive = true,
                        HireDate = DateTime.UtcNow.AddMonths(-6),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }

            // =========================
            // Payroll Transactions
            // =========================
            if (!await context.PayrollTransactions.AnyAsync())
            {
                var employees = await context.Employees.ToListAsync();
                var now = DateTime.Now;

                foreach (var emp in employees)
                {
                    context.PayrollTransactions.Add(new PayrollTransaction
                    {
                        EmployeeId = emp.Id,
                        Year = now.Year,
                        Month = now.Month,
                        BasicSalary = emp.BasicSalary,
                        Allowances = 100,
                        OtherDeductions = 50,
                        NetSalary = emp.BasicSalary + 100 - 50,
                        CreatedAt = DateTime.UtcNow,
                        IsClosed = false
                    });
                }
                await context.SaveChangesAsync();
            }
        }

    }
}