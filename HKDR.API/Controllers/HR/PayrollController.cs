using HKDR.API.Services.HR;
using HKDR.Common.DTOs.HR;
using HKDR.Common.DTOs.HR.EmployeesDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HKDR.API.Controllers.HR
{
    [Authorize]
    [ApiController]
    [Route("api/hr/payroll")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        // ===============================
        // GET: api/hr/payroll?year=2026&month=1
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetByMonth(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var payrolls = await _payrollService.GetByMonthAsync(year, month);
            return Ok(payrolls);
        }

        // ===============================
        // POST: api/hr/payroll/generate?month=2026-01-01
        // Generate payroll for ALL employees
        // ===============================
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePayrollForAll([FromQuery] DateTime month)
        {
            if (month == default)
                return BadRequest("Month is required");

            try
            {
                await _payrollService.GeneratePayrollForAllAsync(month);
                return Ok(new { Message = "Payroll generated successfully" });
            }
            catch (Exception ex)
            {
                // debug
                return StatusCode(500, ex.Message);
            }
        }


        // ===============================
        // GET: api/hr/payroll/{employeeId}/payslip?month=2026-01-01
        // Generate payslip (DTO only)
        // ===============================
        [HttpGet("{employeeId}/payslip")]
        public async Task<IActionResult> GetPayslip(
            int employeeId,
            [FromQuery] DateTime month)
        {
            var payslip =
                await _payrollService.GeneratePayslipAsync(employeeId, month);

            return Ok(payslip);
        }

        // ===============================
        // POST: api/hr/payroll/bonus
        // ===============================
        [HttpPost("bonus")]
        public async Task<IActionResult> AddBonus(
            [FromBody] AddBonusDto dto)
        {
            await _payrollService.AddBonusAsync(
                dto.EmployeeId,
                dto.Amount,
                dto.Reason);

            return Ok(new { Message = "Bonus added successfully" });
        }

        // ===============================
        // POST: api/hr/payroll/close-month?month=2026-01-01
        // ===============================
        [HttpPost("close-month")]
        public async Task<IActionResult> CloseMonth(
            [FromQuery] DateTime month)
        {
            await _payrollService.ClosePayrollMonthAsync(month);
            return Ok(new { Message = "Payroll month closed" });
        }
    }
}
