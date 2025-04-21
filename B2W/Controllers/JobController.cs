using B2W.Models.Dto;
using B2W.Models.Jop;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Post Jop
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostJop([FromBody] JopDto jopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jop = new Jop
            {
                Title = jopDto.Title,
                Description = jopDto.Description,
                Requirments = jopDto.Requirments,
                AboutCompany = jopDto.AboutCompany,
                JopType = jopDto.JopType,
                Level = jopDto.Level,
                WorkingModel = jopDto.WorkingModel,
                Salary = jopDto.Salary,
                UserId = jopDto.UserId
            };

            _context.Jops.Add(jop);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJopById), new { id = jop.JopId }, jop);
        }

        [HttpGet("company/{companyProfileId}")]
        public async Task<IActionResult> GetCompanyJobs(int companyProfileId)
        {
            var jobs = await _context.Jops
                .Where(j => j.CompanyProfileId == companyProfileId)
                .ToListAsync();

            if (jobs == null || !jobs.Any())
                return NotFound("No jobs found for this company.");

            return Ok(jobs);
        }
        // 2. Edit Jop
        [HttpPut("{id}")]
        public async Task<IActionResult> EditJop(int id, [FromBody] JopDto jopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jop = await _context.Jops.FindAsync(id);
            if (jop == null)
            {
                return NotFound();
            }

            jop.Title = jopDto.Title;
            jop.Description = jopDto.Description;
            jop.Requirments = jopDto.Requirments;
            jop.AboutCompany = jopDto.AboutCompany;
            jop.JopType = jopDto.JopType;
            jop.Level = jopDto.Level;
            jop.WorkingModel = jopDto.WorkingModel;
            jop.Salary = jopDto.Salary;
            jop.UserId = jopDto.UserId;

            _context.Jops.Update(jop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 3. Get Jop By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJopById(int id)
        {
            var jop = await _context.Jops
                .Include(j => j.ApplicationUser)
                .FirstOrDefaultAsync(j => j.JopId == id);

            if (jop == null)
            {
                return NotFound();
            }

            var jopDto = new JopDto
            {
                Title = jop.Title,
                Description = jop.Description,
                Requirments = jop.Requirments,
                AboutCompany = jop.AboutCompany,
                JopType = jop.JopType,
                Level = jop.Level,
                WorkingModel = jop.WorkingModel,
                Salary = jop.Salary,
                UserId = jop.UserId
            };

            return Ok(jopDto);
        }

        // 4. Get All Jops
        [HttpGet]
        public async Task<IActionResult> GetAllJops()
        {
            var jops = await _context.Jops
                .Include(j => j.ApplicationUser)
                .Select(j => new
                {
                    j.JopId,
                    Title = j.Title,
                    Description = j.Description,
                    Requirments = j.Requirments,
                    AboutCompany = j.AboutCompany,
                    JopType = j.JopType,
                    Level = j.Level,
                    WorkingModel = j.WorkingModel,
                    Salary = j.Salary,
                    UserId = j.UserId
                })
                .ToListAsync();

            return Ok(jops);
        }

        // 5. Delete Jop
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJop(int id)
        {
            var jop = await _context.Jops.FindAsync(id);
            if (jop == null)
            {
                return NotFound();
            }

            _context.Jops.Remove(jop);
            await _context.SaveChangesAsync();

            return Ok("Job deleted successfully");
        }
    }
}
