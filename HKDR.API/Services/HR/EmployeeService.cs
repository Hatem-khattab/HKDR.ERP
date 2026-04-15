using HKDR.API.Services.HR;
using HKDR.Common.DTOs.HR.EmployeesDto;
using HKDR.DomainEntities.Entities.HR;
using HKDR.Repository.IRepository;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // ========================
    // Get All
    // ========================
    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FullName = e.FullName,
            BasicSalary = e.BasicSalary
        }).ToList();
    }

    // ========================
    // Get By Id
    // ========================
    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository
            .GetByIdWithDepartmentAsync(id);

        if (employee == null)
            return null;

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            BasicSalary = employee.BasicSalary,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department.Name
        };
    }

    // ========================
    // Create
    // ========================
    public async Task<int> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            EmployeeNumber = dto.EmployeeNumber,
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DepartmentId = dto.DepartmentId,
            JobTitle = dto.JobTitle,
            HireDate = dto.HireDate,
            BasicSalary = dto.BasicSalary,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveAsync();

        return employee.Id;
    }

    // ========================
    // Update
    // ========================
    public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return false;

        employee.FullName = dto.FullName;
        employee.BasicSalary = dto.BasicSalary;
        employee.DepartmentId = dto.DepartmentId;
        employee.UpdatedAt = DateTime.UtcNow;

        _employeeRepository.Update(employee);
        await _employeeRepository.SaveAsync();

        return true;
    }

    // ========================
    // Delete
    // ========================
    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return false;

        _employeeRepository.Remove(employee);
        await _employeeRepository.SaveAsync();

        return true;
    }
}
