using B2W.Models.Dto;
using B2W.Models.UserCertifications;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
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


        // Add Certification
        [HttpPost("AddCertification")]
        public async Task<IActionResult> AddCertification([FromForm] UserCertificationAddDto certificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string imagePath = null;
            if (certificationDto.Image != null)
            {
                imagePath = await SaveImageAsync(certificationDto.Image);
            }

            var certification = new UserCertification
            {
                UserId = certificationDto.UserId,
                Description = certificationDto.Description,
                CreatedAt = DateTime.UtcNow,
                Image = imagePath
            };

            _context.userCertifications.Add(certification);
            await _context.SaveChangesAsync();

            return Ok(certification);
        }

        //Get All Certifications
        [HttpGet("GetAllCertifications")]
        public async Task<IActionResult> GetAllCertifications()
        {
            var certifications = await _context.userCertifications.ToListAsync();
            return Ok(certifications);
        }


        // Get Certification by id
        [HttpGet("GetCertification/{id}")]
        public async Task<IActionResult> GetCertification(int id)
        {
            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound("Certification Not Found.");
            }
            return Ok(certification);
        }


        // Edit Certification
        [HttpPut("EditCertification/{id}")]
        public async Task<IActionResult> EditCertification(int id, [FromForm] UserCertificationEditDto certificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound("Certification Not Found.");
            }

            certification.Description = certificationDto.Description;
            certification.UpdatedAt = DateTime.UtcNow;

            if (certificationDto.Image != null)
            {
                if (!string.IsNullOrEmpty(certification.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot", certification.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                certification.Image = await SaveImageAsync(certificationDto.Image);
            }

            _context.userCertifications.Update(certification);
            await _context.SaveChangesAsync();

            return Ok(certification);
        }



        // Delete Certification
        [HttpDelete("DeleteCertification/{id}")]
        public async Task<IActionResult> DeleteCertification(int id)
        {
            var certification = await _context.userCertifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound("Certification Not Found");
            }

            if (!string.IsNullOrEmpty(certification.Image))
            {
                var imagePath = Path.Combine("wwwroot", certification.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.userCertifications.Remove(certification);
            await _context.SaveChangesAsync();

            return Ok("Certification Deleted");
        }

        // Save image
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var imagesPath = Path.Combine("wwwroot", "certifications");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(imagesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/certifications/{fileName}";
        }
    }
}
