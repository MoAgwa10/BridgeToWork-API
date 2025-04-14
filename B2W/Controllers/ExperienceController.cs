using B2W.Dto;
using B2W.Models;
using B2W.Models.Authentication;
using B2W.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ExperienceController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        [HttpGet("GetExperiences")]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
        {
            var experiences = await _db.Experiences
                .Select(exp => new ExperienceDto
                {
                    Id = exp.Id,
                    JobTitle = exp.JobTitle,
                    OrganizationName = exp.OrganizationName,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    ApplicationUserId = exp.ApplicationUserId
                })
                .ToListAsync();

            return Ok(experiences);
        }

       
        [HttpGet("{applicationUserId}")]
        public async Task<ActionResult<ExperienceDto>> GetExperienceByUser(string applicationUserId)
        {
            var experience = await _db.Experiences
                .Where(e => e.ApplicationUserId == applicationUserId)
                .FirstOrDefaultAsync();

            if (experience == null)
                return NotFound("No experience record found for this user.");

            var experienceDto = new ExperienceDto
            {
                Id = experience.Id,
                JobTitle = experience.JobTitle,
                OrganizationName = experience.OrganizationName,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                ApplicationUserId = experience.ApplicationUserId
            };

            return Ok(experienceDto);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddExperience([FromBody] ExperienceDto experienceDto)
        {
            var experience = new Experience
            {
                JobTitle = experienceDto.JobTitle,
                OrganizationName = experienceDto.OrganizationName,
                StartDate = experienceDto.StartDate,
                EndDate = experienceDto.EndDate,
                ApplicationUserId = experienceDto.ApplicationUserId 
            };

            _db.Experiences.Add(experience);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExperienceByUser), new { applicationUserId = experience.ApplicationUserId }, experienceDto);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> EditExperience(int id, [FromBody] ExperienceDto experienceDto)
        {
            var experience = await _db.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound("Experience record not found.");

            experience.JobTitle = experienceDto.JobTitle;
            experience.OrganizationName = experienceDto.OrganizationName;
            experience.StartDate = experienceDto.StartDate;
            experience.EndDate = experienceDto.EndDate;

            await _db.SaveChangesAsync();
            return Ok("Experience record updated successfully.");
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveExperience(int id)
        {
            var experience = await _db.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound("Experience record not found.");

            _db.Experiences.Remove(experience);
            await _db.SaveChangesAsync();

            return Ok("Experience record deleted successfully.");
        }
    }
}