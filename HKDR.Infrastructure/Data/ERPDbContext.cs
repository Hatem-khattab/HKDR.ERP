using HKDR.DomainEntities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HKDR.Infrastructure.Abstractions;
using HKDR.DomainEntities.Entities.HR;
using HKDR.DomainEntities.Entities.Core;

namespace HKDR.Infrastructure.Data
{
    public class ERPDbContext
        : IdentityDbContext<ApplicationUser>
    {
        private readonly ICurrentCompanyService _currentCompany;

        public ERPDbContext(DbContextOptions<ERPDbContext> options, 
            ICurrentCompanyService currentCompany)

            : base(options) {
            _currentCompany = currentCompany;
        }

        // 🔑 SaaS Core
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PayrollTransaction> PayrollTransactions { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Performance> Performances { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ⭐ لا تنحذف

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ERPDbContext).Assembly
            );

            // 🔥 Global Company Filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ERPDbContext)
                        .GetMethod(nameof(SetCompanyFilter),
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder, this });
                }
            }

            modelBuilder.Entity<Employee>()
               .HasMany(e => e.Payrolls)
               .WithOne(p => p.Employee)
               .HasForeignKey(p => p.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>()
               .HasMany(e => e.Leaves)
               .WithOne(l => l.Employee)
               .HasForeignKey(l => l.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Performances)
                .WithOne(perf => perf.Employee)
                .HasForeignKey(perf => perf.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);



        }
        private static void SetCompanyFilter<TEntity>(
           ModelBuilder modelBuilder,
           ERPDbContext context
       ) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e => e.CompanyId == context._currentCompany.CompanyId);
        }



    }
}
