using B2W.Dto;
using B2W.Models;
using B2W.Models.Authentication;
using B2W.Models.Dto;
using B2W.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1️⃣ جلب جميع الملفات الشخصية لكل المستخدمين
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUserProfiles()
        {
            var profiles = await _context.UserProfiles
                                         .Select(u => new UserProfileDto
                                         {
                                             Id = u.Id,
                                             FirstName = u.FirstName,
                                             LastName = u.LastName,
                                             Email = u.Email,
                                             Gender = u.Gender,
                                             JobTitle = u.JobTitle,
                                             ProfileImageUrl = u.ProfileImageUrl,
                                             JobType = u.JobType,
                                             WorkModel = u.WorkModel,
                                             ExperienceLevel = u.ExperienceLevel,
                                             DesiredJobTitle = u.DesiredJobTitle,
                                             DisabilityType = u.DisabilityType,
                                             FontSize = u.FontSize,
                                             DarkMode = u.DarkMode,
                                             ApplicationUserId = u.ApplicationUserId
                                         })
                                         .ToListAsync();

            return Ok(profiles);
        }

        // ✅ 2️⃣ جلب ملف مستخدم معين عن طريق `ApplicationUserId`
        [HttpGet("ByUser/{ApplicationUserId}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileByUserId(string ApplicationUserId)
        {
            var profile = await _context.UserProfiles
                                        .Where(u => u.ApplicationUserId == ApplicationUserId)
                                        .Select(u => new UserProfileDto
                                        {
                                            Id = u.Id,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Email = u.Email,
                                            Gender = u.Gender,
                                            JobTitle = u.JobTitle,
                                            ProfileImageUrl = u.ProfileImageUrl,
                                            JobType = u.JobType,
                                            WorkModel = u.WorkModel,
                                            ExperienceLevel = u.ExperienceLevel,
                                            DesiredJobTitle = u.DesiredJobTitle,
                                            DisabilityType = u.DisabilityType,
                                            FontSize = u.FontSize,
                                            DarkMode = u.DarkMode,
                                            ApplicationUserId = u.ApplicationUserId
                                        })
                                        .FirstOrDefaultAsync();

            if (profile == null)
                return NotFound("User profile not found.");

            return Ok(profile);
        }

        // ✅ 3️⃣ جلب ملف شخصي معين عن طريق `id`
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileById(int id)
        {
            var profile = await _context.UserProfiles
                                        .Where(u => u.Id == id)
                                        .Select(u => new UserProfileDto
                                        {
                                            Id = u.Id,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Email = u.Email,
                                            Gender = u.Gender,
                                            JobTitle = u.JobTitle,
                                            ProfileImageUrl = u.ProfileImageUrl,
                                            JobType = u.JobType,
                                            WorkModel = u.WorkModel,
                                            ExperienceLevel = u.ExperienceLevel,
                                            DesiredJobTitle = u.DesiredJobTitle,
                                            DisabilityType = u.DisabilityType,
                                            FontSize = u.FontSize,
                                            DarkMode = u.DarkMode,
                                            ApplicationUserId = u.ApplicationUserId
                                        })
                                        .FirstOrDefaultAsync();

            if (profile == null)
                return NotFound("User profile not found.");

            return Ok(profile);
        }

        // ✅ 4️⃣ إضافة ملف شخصي جديد
        [HttpPost]
        public async Task<ActionResult<UserProfileDto>> PostUserProfile(UserProfileDto userProfileDto)
        {
            var userProfile = new UserProfile
            {
                FirstName = userProfileDto.FirstName,
                LastName = userProfileDto.LastName,
                Email = userProfileDto.Email,
                Gender = userProfileDto.Gender,
                JobTitle = userProfileDto.JobTitle,
                ProfileImageUrl = userProfileDto.ProfileImageUrl,
                JobType = userProfileDto.JobType,
                WorkModel = userProfileDto.WorkModel,
                ExperienceLevel = userProfileDto.ExperienceLevel,
                DesiredJobTitle = userProfileDto.DesiredJobTitle,
                DisabilityType = userProfileDto.DisabilityType,
                FontSize = userProfileDto.FontSize,
                DarkMode = userProfileDto.DarkMode,
                ApplicationUserId = userProfileDto.ApplicationUserId
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserProfileById), new { id = userProfile.Id }, userProfile);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUserProfile(int id, [FromBody] Dictionary<string, object> updates)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
                return NotFound("User profile not found.");

            // ✅ تحديث الحقول فقط إذا تم إرسالها
            foreach (var update in updates)
            {
                var property = typeof(UserProfile).GetProperty(update.Key);
                if (property != null)
                {
                    property.SetValue(userProfile, Convert.ChangeType(update.Value, property.PropertyType));
                }
            }

            _context.Entry(userProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ 6️⃣ حذف ملف شخصي
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
                return NotFound("User profile not found.");

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ 7️⃣ رفع صورة الملف الشخصي
        [HttpPost("UploadProfileImage/{id}")]
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
                return NotFound("User profile not found.");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine("wwwroot/uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            userProfile.ProfileImageUrl = $"/uploads/{fileName}";
            _context.Entry(userProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = userProfile.ProfileImageUrl });
        }
    }
}
