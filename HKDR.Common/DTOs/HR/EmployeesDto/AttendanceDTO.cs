public class AttendanceDTO
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public bool IsAbsent { get; set; }
    public bool IsLate { get; set; }
}