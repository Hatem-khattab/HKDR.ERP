using HKDR.API.Services.HR;
using HKDR.Common.DTOs.HR.EmployeesDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HKDR.API.Controllers.HR
{
    [Authorize]
    [ApiController]
    [Route("api/hr/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/hr/employees
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeeService.GetAllAsync());
        }

        // GET: api/hr/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/hr/employees
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var id = await _employeeService.CreateAsync(dto);
            return Ok(new { EmployeeId = id });
        }

        // PUT: api/hr/employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            var updated = await _employeeService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/hr/employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}