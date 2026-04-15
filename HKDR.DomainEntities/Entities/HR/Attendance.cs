// الحضور والانصراف
using HKDR.DomainEntities.Entities.HR;
using System.ComponentModel.DataAnnotations;

public class Attendance
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public DateTime Date { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    public bool IsAbsent { get; set; }
    public bool IsLate { get; set; }
}