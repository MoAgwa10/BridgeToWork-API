using System.Reflection;
using B2W.Dto;
using B2W.Models;
using B2W.Models.Authentication;
using B2W.Models.Dto;
using B2W.Dto.UserProfileDtos;
using B2W.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B2W.Models.Dto.UserProfileDto;

namespace B2W.Controllers.UserProfileController
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

        // Get All UserProfiles
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
                    JobType = u.JobType,
                    WorkModel = u.WorkModel,
                    ExperienceLevel = u.ExperienceLevel,
                    DesiredJobTitle = u.DesiredJobTitle,
                    DisabilityType = u.DisabilityType,
                    FontSize = u.FontSize,
                    DarkMode = u.DarkMode,
                    ApplicationUserId = u.ApplicationUserId
                }).ToListAsync();

            return Ok(profiles);
        }

        [HttpGet("ByUser/{ApplicationUserId}")]
        public async Task<ActionResult<UserProfileDto>> GetByUserId(string ApplicationUserId)
        {
            var profile = await _context.UserProfiles
                .Include(u => u.Experiences)
                .Include(u => u.Educations)
                .Include(u => u.Skills)
                .Include(u => u.Projects)
                .Include(u => u.MillStones)
                .Include(u => u.UserCertifications)
                .Include(u => u.UserCv)
                .Where(u => u.ApplicationUserId == ApplicationUserId)
                .FirstOrDefaultAsync();

            if (profile == null)
                return NotFound();

            // Get latest profile picture for the user
            var profilePicture = await _context.userProfilePictures
                .Where(p => p.UserId == ApplicationUserId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            var dto = new UserProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                Gender = profile.Gender,
                JobTitle = profile.JobTitle,
                JobType = profile.JobType,
                WorkModel = profile.WorkModel,
                ExperienceLevel = profile.ExperienceLevel,
                DesiredJobTitle = profile.DesiredJobTitle,
                DisabilityType = profile.DisabilityType,
                FontSize = profile.FontSize,
                DarkMode = profile.DarkMode,
                ApplicationUserId = profile.ApplicationUserId,

                Experiences = profile.Experiences.Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    JobTitle = e.JobTitle,
                    OrganizationName = e.OrganizationName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId,

                }).ToList(),

                Educations = profile.Educations.Select(ed => new EducationDto
                {
                    Id = ed.Id,
                    University = ed.University,
                    Faculty = ed.Faculty,
                    Degree = ed.Degree,
                    StartDate = ed.StartDate,
                    EndDate = ed.EndDate,
                    UserProfileId = ed.UserProfileId,
                }).ToList(),

                Skills = profile.Skills.Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId,
                }).ToList(),

                Projects = profile.Projects.Select(p => new ProjectsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserProfileId = p.UserProfileId,
                }).ToList(),

                MillStones = profile.MillStones.Select(m => new MillStoneDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Company = m.Company,
                    Date = m.Date,
                    UserProfileId = m.UserProfileId,
                }).ToList(),

                UserProfilePicture = profilePicture != null ? new UserProfilePictureDto
                {
                    Id = profilePicture.UserProfilePictureId,
                    Url = $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.Image)}"
                } : null,

                UserCertifications = profile.UserCertifications.Select(cert => new UserCertificationDto
                {
                    CertificationId = cert.CertificationId,
                    Description = cert.Description,
                    Image = cert.Image,
                    CreatedAt = cert.CreatedAt,
                    UpdatedAt = cert.UpdatedAt,
                    UserProfileId = cert.UserProfileId
                }).ToList(),
                Cv = profile.UserCv != null ? new CvDto
                {
                    CvFilePath = profile.UserCv.CvFilePath,
                    UserProfileId = profile.UserCv.UserProfileId
                } : null



            };


            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetById(int id)
        {
            var profile = await _context.UserProfiles
                .Include(u => u.Experiences)
                .Include(u => u.Educations)
                .Include(u => u.Skills)
                .Include(u => u.Projects)
                .Include(u => u.MillStones)
                .Include(u => u.UserCertifications)
                .Include(u => u.UserCv)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (profile == null)
                return NotFound();

            var profilePicture = await _context.userProfilePictures
                .Where(p => p.UserId == profile.ApplicationUserId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            var dto = new UserProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                Gender = profile.Gender,
                JobTitle = profile.JobTitle,
                JobType = profile.JobType,
                WorkModel = profile.WorkModel,
                ExperienceLevel = profile.ExperienceLevel,
                DesiredJobTitle = profile.DesiredJobTitle,
                DisabilityType = profile.DisabilityType,
                FontSize = profile.FontSize,
                DarkMode = profile.DarkMode,
                ApplicationUserId = profile.ApplicationUserId,

                Experiences = profile.Experiences.Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    JobTitle = e.JobTitle,
                    OrganizationName = e.OrganizationName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    UserProfileId = e.UserProfileId,
                }).ToList(),

                Educations = profile.Educations.Select(ed => new EducationDto
                {
                    Id = ed.Id,
                    University = ed.University,
                    Faculty = ed.Faculty,
                    Degree = ed.Degree,
                    StartDate = ed.StartDate,
                    EndDate = ed.EndDate,
                    UserProfileId = ed.UserProfileId,
                }).ToList(),

                Skills = profile.Skills.Select(s => new SkillsDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    UserProfileId = s.UserProfileId,
                }).ToList(),

                Projects = profile.Projects.Select(p => new ProjectsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserProfileId = p.UserProfileId,
                }).ToList(),

                MillStones = profile.MillStones.Select(m => new MillStoneDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Company = m.Company,
                    Date = m.Date,
                    UserProfileId = m.UserProfileId,
                }).ToList(),

                UserCertifications = profile.UserCertifications.Select(cert => new UserCertificationDto
                {
                    CertificationId = cert.CertificationId,
                    Description = cert.Description,
                    Image = cert.Image,
                    CreatedAt = cert.CreatedAt,
                    UpdatedAt = cert.UpdatedAt,
                    UserProfileId = cert.UserProfileId
                }).ToList(),

                Cv = profile.UserCv != null ? new CvDto
                {
                    CvFilePath = profile.UserCv.CvFilePath,
                    UserProfileId = profile.UserCv.UserProfileId
                } : null,

                UserProfilePicture = profilePicture != null ? new UserProfilePictureDto
                {
                    Id = profilePicture.UserProfilePictureId,
                    Url = $"data:image/jpeg;base64,{Convert.ToBase64String(profilePicture.Image)}"
                } : null
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProfileCreateDto dto)
        {
            var profile = new UserProfile
            {
                
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Gender = dto.Gender,
                JobTitle = dto.JobTitle,
                JobType = dto.JobType,
                WorkModel = dto.WorkModel,
                ExperienceLevel = dto.ExperienceLevel,
                DesiredJobTitle = dto.DesiredJobTitle,
                DisabilityType = dto.DisabilityType,
                FontSize = dto.FontSize,
                DarkMode = dto.DarkMode,
                ApplicationUserId = dto.ApplicationUserId
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            // ربط UserProfileId في جدول المستخدم
            var user = await _context.Users.FindAsync(dto.ApplicationUserId);
            if (user != null)
            {
                user.UserProfileId = profile.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            var responseDto = new UserProfileCreateDto
            {
                id=profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                Gender = profile.Gender,
                JobTitle = profile.JobTitle,
                JobType = profile.JobType,
                WorkModel = profile.WorkModel,
                ExperienceLevel = profile.ExperienceLevel,
                DesiredJobTitle = profile.DesiredJobTitle,
                DisabilityType = profile.DisabilityType,
                FontSize = profile.FontSize,
                DarkMode = profile.DarkMode,
                ApplicationUserId = profile.ApplicationUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, responseDto);
        }


        // Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Dictionary<string, object> updates)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            foreach (var update in updates)
            {
                var prop = typeof(UserProfile).GetProperty(update.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(profile, Convert.ChangeType(update.Value, prop.PropertyType));
                }
            }

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
