using B2W.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.CompanyProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyEmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyEmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.CompanyEmployees
                .Include(e => e.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Where(e => e.UserProfile != null)
                .Select(e => new
                {
                    e.Id,
                    e.CompanyProfileId,
                    e.UserProfileId,
                    FullName = e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                    Position = e.UserProfile.JobType,
                    ImageUrl = e.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(employees);
        }

        // ✅ Get Employee by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.CompanyEmployees
                .Include(e => e.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.CompanyProfileId,
                    e.UserProfileId,
                    FullName = e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                    Position = e.UserProfile.JobType,
                    ImageUrl = e.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // ✅ Get Employees by CompanyProfileId
        [HttpGet("company/{companyProfileId}")]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyProfileId)
        {
            var employees = await _context.CompanyEmployees
                .Include(e => e.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Where(e => e.CompanyProfileId == companyProfileId)
                .Select(e => new
                {
                    e.Id,
                    e.CompanyProfileId,
                    e.UserProfileId,
                    FullName = e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                    Position = e.UserProfile.JobType,
                    ImageUrl = e.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(employees);
        }

        // ✅ Add Employee
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] CompanyEmployee employee)
        {
            _context.CompanyEmployees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        // ✅ Update Employee
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CompanyEmployee updated)
        {
            var employee = await _context.CompanyEmployees.FindAsync(id);
            if (employee == null)
                return NotFound();

            employee.UserProfileId = updated.UserProfileId;
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        // ✅ Delete Employee
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.CompanyEmployees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.CompanyEmployees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
