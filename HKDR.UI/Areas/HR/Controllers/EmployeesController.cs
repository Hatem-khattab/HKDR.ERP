using HKDR.Common.DTOs.HR.EmployeesDto;
using HKDR.UI.Areas.HR.Models.Employee;
using HKDR.UI.Services.HR.Department;
using HKDR.UI.Services.HR.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
[Area("HR")]
public class EmployeesController : Controller
{
    private readonly IEmployeeApiService _employeeService;
    private readonly IDepartmentApiService _departmentService;

    public EmployeesController(
        IEmployeeApiService employeeService,
        IDepartmentApiService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    // =========================
    // LIST + SEARCH
    // =========================
    public async Task<IActionResult> Index(string search)
    {
        try
        {
            var employees = await _employeeService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                employees = employees
                    .Where(e => e.FullName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(employees);
        }
        catch
        {
            TempData["Error"] = "Failed to load employees.";
            return View(new List<EmployeeViewModel>());
        }
    }
    public IActionResult Payroll()
    {
        var model = new PayrollViewModel
        {
            SelectedMonth = DateTime.Today,
            Payrolls = new List<PayrollDto>() 
        };

        return View(model);
    }

    // =========================
    // CREATE
    // =========================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateEmployeeViewModel
        {
            Departments = await LoadDepartmentsAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Departments = await LoadDepartmentsAsync();
            return View(model);
        }

        try
        {
            await _employeeService.CreateAsync(model);
            TempData["Success"] = "Employee created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Failed to create employee.";
            model.Departments = await LoadDepartmentsAsync();
            return View(model);
        }
    }

    // =========================
    // DETAILS
    // =========================
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound();

        return View(employee);
    }

    // =========================
    // EDIT
    // =========================
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
       

        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound();
        var model = new EditEmployeeViewModel
        {
            Id = employee.Id,
            FullName = employee.FullName,
            BasicSalary = employee.BasicSalary,
            DepartmentId = employee.DepartmentId,
            Departments = await LoadDepartmentsAsync()
        };
        Console.WriteLine("POST HIT");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditEmployeeViewModel model)
    {
      
        if (!ModelState.IsValid)
        {
            model.Departments = await LoadDepartmentsAsync();
            return View(model);
        }
        Console.WriteLine("POST HIT");

        try
        {
            await _employeeService.UpdateAsync(model);
            TempData["Success"] = "Employee updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "Failed to update employee.";
            model.Departments = await LoadDepartmentsAsync();
            return View(model);
        }
    }

    // =========================
    // DELETE (CONFIRMATION)
    // =========================
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound();

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _employeeService.DeleteAsync(id);
            TempData["Success"] = "Employee deleted successfully.";
        }
        catch
        {
            TempData["Error"] = "Failed to delete employee.";
        }

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // SALARY DEFINITION (PLACEHOLDER)
    // =========================
    public IActionResult SalaryDef()
    {
        return View();
    }

    // =========================
    // PRIVATE HELPERS
    // =========================
    private async Task<List<SelectListItem>> LoadDepartmentsAsync()
    {
        var departments = await _departmentService.GetAllAsync();

        return departments.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name
        }).ToList();
    }
}
