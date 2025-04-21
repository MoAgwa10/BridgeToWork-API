using B2W.Models.CompanyProfile;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.CompanyProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessibilityFeatureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccessibilityFeatureController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.AccessibilityFeatures.ToListAsync();
            return Ok(data);
        }

        [HttpGet("by-company/{companyProfileId}")]
        public async Task<IActionResult> GetByCompanyProfileId(int companyProfileId)
        {
            var data = await _context.AccessibilityFeatures
                .Where(x => x.CompanyProfileId == companyProfileId)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.AccessibilityFeatures.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AccessibilityFeatureDto dto)
        {
            var entity = new AccessibilityFeature
            {
                FeatureName = dto.FeatureName,
                CompanyProfileId = dto.CompanyProfileId
            };

            _context.AccessibilityFeatures.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, AccessibilityFeatureDto dto)
        {
            var entity = await _context.AccessibilityFeatures.FindAsync(id);
            if (entity == null) return NotFound();

            entity.FeatureName = dto.FeatureName;
            entity.CompanyProfileId = dto.CompanyProfileId;

            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.AccessibilityFeatures.FindAsync(id);
            if (entity == null) return NotFound();

            _context.AccessibilityFeatures.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }
    }

}
