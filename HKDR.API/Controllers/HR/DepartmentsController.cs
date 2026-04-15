using HKDR.DomainEntities.Entities;
using HKDR.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKDR.API.Controllers.HR
{
    [Authorize]
    [ApiController]
    [Route("api/hr/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ERPDbContext _context;

        public DepartmentsController(ERPDbContext context)
        {
            _context = context;
        }

        // GET: api/hr/departments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments);
        }

        // POST: api/hr/departments
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return Ok(department);
        }
    }
}
