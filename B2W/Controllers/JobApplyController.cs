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
    public class JobApplyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobApplyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "User")]
        [HttpPost("Apply")]
        public async Task<IActionResult> Apply([FromForm] JopApplyDto jopApplyDto, IFormFile cvFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // حفظ ملف السيرة الذاتية
            string cvPath = null;
            if (cvFile != null)
            {
                cvPath = await SaveCvAsync(cvFile);
            }

            var jopApply = new JopApply
            {
                JopId = jopApplyDto.JopId,
                UserId = jopApplyDto.UserId,
                FullName = jopApplyDto.FullName,
                Email = jopApplyDto.Email,
                CvFile = cvPath,
                AnyComment = jopApplyDto.AnyComment
            };

            _context.JopApplies.Add(jopApply);
            await _context.SaveChangesAsync();

            return Ok(jopApply);
        }

        // حفظ ملف السيرة الذاتية
        private async Task<string> SaveCvAsync(IFormFile cvFile)
        {
            var cvPath = Path.Combine("wwwroot", "cvs");
            if (!Directory.Exists(cvPath))
            {
                Directory.CreateDirectory(cvPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvFile.FileName);
            var filePath = Path.Combine(cvPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(stream);
            }

            return $"/cvs/{fileName}";
        }









        [HttpGet("GetAllApplications")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _context.JopApplies
                .Include(ja => ja.ApplicationUser)
                .Select(ja => new 
                {
                    ja.JopApplyId,
                    JopId = ja.JopId,
                    UserId = ja.UserId,
                    FullName = ja.FullName,
                    Email = ja.Email,
                    ja.CvFile,
                    Anycomment=ja.AnyComment

                })
                .ToListAsync();

            return Ok(applications);
        }

        [HttpGet("GetApplication/{id}")]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            var jopapply = await _context.JopApplies
                .Include(ja => ja.ApplicationUser)
                .FirstOrDefaultAsync(ja => ja.JopApplyId == id);

            if (jopapply == null)
            {
                return NotFound("Application not found.");
            }

            // إنشاء كائن JSON مسطح
            var result = new
            {
                jopApplyId = jopapply.JopApplyId, // إضافة JopApplyId
                jopId = jopapply.JopId,
                userId = jopapply.UserId,
                fullName = jopapply.FullName,
                email = jopapply.Email,
                cvFile = jopapply.CvFile,
                Anycomment=jopapply.AnyComment
            };

            return Ok(result);
        }











        [HttpPut("EditApplication/{id}")]
        public async Task<IActionResult> EditApplication(int id, [FromForm] JopApplyDto updateJopApplyDto, IFormFile newCvFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var application = await _context.JopApplies.FindAsync(id);
            if (application == null)
            {
                return NotFound("Application not found.");
            }

            application.FullName = updateJopApplyDto.FullName;
            application.Email = updateJopApplyDto.Email;

            if (newCvFile != null)
            {
                if (!string.IsNullOrEmpty(application.CvFile))
                {
                    var oldCvPath = Path.Combine("wwwroot", application.CvFile.TrimStart('/'));
                    if (System.IO.File.Exists(oldCvPath))
                    {
                        System.IO.File.Delete(oldCvPath);
                    }
                }
                application.CvFile = await SaveCvAsync(newCvFile);
            }

            _context.JopApplies.Update(application);
            await _context.SaveChangesAsync();

            var result = new
            {
                jopApplyId = application.JopApplyId,
                jopId = application.JopId,
                userId = application.UserId,
                fullName = application.FullName,
                email = application.Email,
                cvFile = application.CvFile,
                Anycomment=application.AnyComment
            };

            return Ok(result);
        }

        [HttpDelete("DeleteApplication/{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.JopApplies.FindAsync(id);
            if (application == null)
            {
                return NotFound("Application not found.");
            }

            if (!string.IsNullOrEmpty(application.CvFile))
            {
                var cvPath = Path.Combine("wwwroot", application.CvFile.TrimStart('/'));
                if (System.IO.File.Exists(cvPath))
                {
                    System.IO.File.Delete(cvPath);
                }
            }

            _context.JopApplies.Remove(application);
            await _context.SaveChangesAsync();

            return Ok("Application deleted successfully.");
        }
    }
}
