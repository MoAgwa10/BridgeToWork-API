using B2W.Models.Dto;
using B2W.Models.UserCertifications;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B2W.Dto.UserProfileDtos;

namespace B2W.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCertificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserCertificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Get all certifications
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var certifications = await _context.userCertifications.ToListAsync();
            return Ok(certifications);
        }

        // ✅ GET: Get certification by Id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
                return NotFound("Certification not found");

            return Ok(certification);
        }

        // ✅ GET: Get certifications by UserProfileId
        [HttpGet("GetByProfile/{userProfileId}")]
        public async Task<IActionResult> GetByProfile(int userProfileId)
        {
            var certifications = await _context.userCertifications
                .Where(c => c.UserProfileId == userProfileId)
                .ToListAsync();

            return Ok(certifications);
        }

        // ✅ POST: Add new certification
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] UserCertificationAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imagePath = null;
            if (dto.Image != null)
            {
                imagePath = await SaveImageAsync(dto.Image);
            }

            var certification = new UserCertification
            {
                UserProfileId = dto.UserProfileId,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                Image = imagePath
            };

            _context.userCertifications.Add(certification);
            await _context.SaveChangesAsync();

            return Ok(certification);
        }

        // ✅ PATCH: Edit certification
        [HttpPatch("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] UserCertificationEditDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
                return NotFound("Certification not found");

            certification.Description = dto.Description;
            certification.UpdatedAt = DateTime.UtcNow;

            if (dto.Image != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(certification.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot", certification.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // Save new image
                certification.Image = await SaveImageAsync(dto.Image);
            }

            _context.userCertifications.Update(certification);
            await _context.SaveChangesAsync();

            return Ok(certification);
        }

        // ✅ DELETE: Delete certification
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
                return NotFound("Certification not found");

            if (!string.IsNullOrEmpty(certification.Image))
            {
                var imagePath = Path.Combine("wwwroot", certification.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.userCertifications.Remove(certification);
            await _context.SaveChangesAsync();

            return Ok("Certification deleted");
        }

        // ✅ Save image helper
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine("wwwroot", "certifications");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/certifications/{fileName}";
        }
    }
}