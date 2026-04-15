namespace HKDR.Common.DTOs.HR.EmployeesDto
{
    public class EmployeeFilterDto
    {
        // Search
        public string? Keyword { get; set; }

        // Filters
        public int? DepartmentId { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
