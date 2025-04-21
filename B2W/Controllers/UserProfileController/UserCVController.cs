using B2W.Models;
using B2W.Models.Dto.UserProfileDto;
using B2W.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCvController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserCvController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cvs = await _context.Cvs
                .Select(c => new CvDto
                {
                    Id = c.Id,
                    UserProfileId = c.UserProfileId,
                    CvFilePath = c.CvFilePath
                }).ToListAsync();

            return Ok(cvs);
        }

        [HttpGet("by-user/{userProfileId}")]
        public async Task<IActionResult> GetByUserProfileId(int userProfileId)
        {
            var cvs = await _context.Cvs
                .Where(c => c.UserProfileId == userProfileId)
                .Select(c => new CvDto
                {
                    Id = c.Id,
                    UserProfileId = c.UserProfileId,
                    CvFilePath = c.CvFilePath
                }).ToListAsync();

            return Ok(cvs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null) return NotFound();

            return Ok(new CvDto
            {
                Id = cv.Id,
                UserProfileId = cv.UserProfileId,
                CvFilePath = cv.CvFilePath
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] UserCvAddDto dto)
        {
            if (dto.CvFile == null || dto.CvFile.Length == 0)
                return BadRequest("CV file is required.");

            var fileName = $"{Guid.NewGuid()}_{dto.CvFile.FileName}";
            var filePath = Path.Combine(_environment.WebRootPath, "cvs", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.CvFile.CopyToAsync(stream);
            }

            var cv = new Cv
            {
                UserProfileId = dto.UserProfileId,
                CvFilePath = Path.Combine("cvs", fileName)
            };

            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();

            return Ok(new { cv.Id, cv.CvFilePath });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UserCvEditDto dto)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null) return NotFound();

            if (dto.CvFile != null && dto.CvFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{dto.CvFile.FileName}";
                var filePath = Path.Combine(_environment.WebRootPath, "cvs", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CvFile.CopyToAsync(stream);
                }

                cv.CvFilePath = Path.Combine("cvs", fileName);
            }

            await _context.SaveChangesAsync();
            return Ok(new { cv.Id, cv.CvFilePath });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null) return NotFound();

            _context.Cvs.Remove(cv);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
