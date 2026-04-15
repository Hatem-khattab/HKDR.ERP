using HKDR.DomainEntities.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HKDR.Infrastructure.Configurations.HR
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.EmployeeNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(e => e.Department)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict); // 🔴 الحل
        }
    }
}
