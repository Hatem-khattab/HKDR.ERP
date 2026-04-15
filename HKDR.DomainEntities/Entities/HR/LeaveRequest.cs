// الإجازات
using HKDR.DomainEntities.Entities.HR;
using System.ComponentModel.DataAnnotations;

public class LeaveRequest
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public string LeaveType { get; set; } // Annual, Sick, Emergency
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int Days => (EndDate - StartDate).Days + 1;

    public string Status { get; set; } // Pending, Approved, Rejected
}