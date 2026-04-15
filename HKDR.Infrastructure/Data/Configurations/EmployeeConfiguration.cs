using HKDR.DomainEntities.Entities;
using HKDR.DomainEntities.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EmployeeConfiguration
    : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(e => e.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasOne<Department>()
               .WithMany()
               .HasForeignKey(e => e.DepartmentId);
    }
}
