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
        public async Task<ActionResult<IEnumerable<CompanyProfilseDto>>> GetAllCompanyProfiles()
        {
            var companies = await _context.CompanyProfiles
                .Select(c => new CompanyProfilseDto
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
        public async Task<ActionResult<CompanyProfilseDto>> GetByUserId(string ApplicationUserId)
        {
            var company = await _context.CompanyProfiles
                .Include(c => c.AccessibilityFeatures)
                .Include(c => c.Reviews)
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == ApplicationUserId);

            if (company == null) return NotFound();

            var dto = new CompanyProfilseDto
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

        // Get by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyProfilseDto>> GetById(int id)
        {
            var company = await _context.CompanyProfiles
                .Where(c => c.CompanyProfileId == id)
                .Select(c => new CompanyProfilseDto
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
                }).FirstOrDefaultAsync();

            if (company == null) return NotFound();
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyProfilseDto dto)
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

            // بعد الحفظ، نربط البروفايل باليوزر
            var user = await _context.Users.FindAsync(dto.ApplicationUserId);
            if (user != null)
            {
                user.CompanyProfileId = entity.CompanyProfileId;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = entity.CompanyProfileId }, entity);
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