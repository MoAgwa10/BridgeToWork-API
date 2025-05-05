using B2W.Models.CompanyProfile;
using B2W.Models.Dto.CompanyProfileDto;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CompanyProfilesDto;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace B2W.Controllers.CompanyProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get All CompanyProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyProfilesDto>>> GetAllCompanyProfiles()
        {
            var companies = await _context.CompanyProfiles
                .Select(c => new CompanyProfilesDto
                {
                    CompanyProfileId = c.CompanyProfileId,
                    CompanyName = c.CompanyName,
                    Email = c.Email,
                    FieldOfWork = c.FieldOfWork,
                    WebsiteUrl = c.WebsiteUrl,
                    SocialMediaLinks = c.SocialMediaLinks,
                    Location = c.Location,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    ApplicationUserId = c.ApplicationUserId
                }).ToListAsync();

            return Ok(companies);
        }

        // Get By ApplicationUserId
        [HttpGet("ByUser/{ApplicationUserId}")]
        public async Task<ActionResult<CompanyProfilesDto>> GetByUserId(string ApplicationUserId)
        {
            var company = await _context.CompanyProfiles
                .Include(c => c.AccessibilityFeatures)
                .Include(c => c.Reviews)
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == ApplicationUserId);

            if (company == null) return NotFound();

            var dto = new CompanyProfilesDto
            {
                CompanyProfileId = company.CompanyProfileId,
                CompanyName = company.CompanyName,
                Email = company.Email,
                FieldOfWork = company.FieldOfWork,
                WebsiteUrl = company.WebsiteUrl,
                SocialMediaLinks = company.SocialMediaLinks,
                Location = company.Location,
                Description = company.Description,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt,
                ApplicationUserId = company.ApplicationUserId,
                AccessibilityFeatures = company.AccessibilityFeatures.Select(a => new AccessibilityFeatureDto
                {
                    Id = a.Id,
                    FeatureName = a.FeatureName,
                    CompanyProfileId = a.CompanyProfileId
                }).ToList(),
                Reviews = company.Reviews.Select(r => new CompanyReviewDto
                {
                    Id = r.Id,
                    Message = r.Message,
                    Rating = r.Rating,
                    CompanyProfileId = r.CompanyProfileId,
                    UserProfileId = r.UserProfileId
                }).ToList(),
                Employees = company.Employees.Select(e => new CompanyEmployeeDto
                {
                    Id = e.Id,
                    FullName = e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                    JobType = e.UserProfile.JobType,
                    UserProfileId = e.UserProfileId
                }).ToList()
            };

            return Ok(dto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyProfilesDto>> GetById(int id)
        {
            var company = await _context.CompanyProfiles
                .Include(c => c.Employees)
                .Include(c => c.Reviews)
                .Include(c => c.AccessibilityFeatures)
                .FirstOrDefaultAsync(c => c.CompanyProfileId == id);

            if (company == null)
                return NotFound();

            var dto = new CompanyProfilesDto
            {
                CompanyProfileId = company.CompanyProfileId,
                CompanyName = company.CompanyName,
                Email = company.Email,
                FieldOfWork = company.FieldOfWork,
                WebsiteUrl = company.WebsiteUrl,
                SocialMediaLinks = company.SocialMediaLinks,
                Location = company.Location,
                Description = company.Description,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt,
                ApplicationUserId = company.ApplicationUserId,

                Employees = company.Employees?.Select(e => new CompanyEmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    JobType = e.JobType,
                    UserProfileId = e.UserProfileId,
                    CompanyProfileId = e.CompanyProfileId
                }).ToList(),

                Reviews = company.Reviews?.Select(r => new CompanyReviewDto
                {
                    Id = r.Id,
                    Message = r.Message,
                    Rating = r.Rating,
                    CompanyProfileId = r.CompanyProfileId,
                    UserProfileId = r.UserProfileId
                }).ToList(),

                AccessibilityFeatures = company.AccessibilityFeatures?.Select(a => new AccessibilityFeatureDto
                {
                    Id = a.Id,
                    FeatureName = a.FeatureName,
                    CompanyProfileId = a.CompanyProfileId
                }).ToList()
            };

            return Ok(dto);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyProfileCreateDto dto)
        {
            var entity = new CompanyProfile
            {
                CompanyName = dto.CompanyName,
                Email = dto.Email,
                FieldOfWork = dto.FieldOfWork,
                WebsiteUrl = dto.WebsiteUrl,
                SocialMediaLinks = dto.SocialMediaLinks,
                Location = dto.Location,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = dto.ApplicationUserId
            };

            _context.CompanyProfiles.Add(entity);
            await _context.SaveChangesAsync();

            // ربط الـ CompanyProfileId بالـ ApplicationUser
            var user = await _context.Users.FindAsync(dto.ApplicationUserId);
            if (user != null)
            {
                user.CompanyProfileId = entity.CompanyProfileId;
                await _context.SaveChangesAsync();
            }

            // تجهيز DTO للرد
            var responseDto = new CompanyProfileCreateDto
            {
                CompanyProfileId = entity.CompanyProfileId,
                CompanyName = entity.CompanyName,
                Email = entity.Email,
                FieldOfWork = entity.FieldOfWork,
                WebsiteUrl = entity.WebsiteUrl,
                SocialMediaLinks = entity.SocialMediaLinks,
                Location = entity.Location,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ApplicationUserId = entity.ApplicationUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.CompanyProfileId }, responseDto);
        }
        // Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var entity = await _context.CompanyProfiles.FindAsync(id);
            if (entity == null) return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(CompanyProfile).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(entity, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.CompanyProfiles.FindAsync(id);
            if (entity == null) return NotFound();

            _context.CompanyProfiles.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}