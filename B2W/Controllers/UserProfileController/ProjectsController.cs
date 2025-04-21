using B2W.Dto.UserProfileDtos;
using B2W.Models.User;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Get all projects
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _context.Projects
                .Select(p => new ProjectsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserProfileId = p.UserProfileId
                }).ToListAsync();

            return Ok(projects);
        }

        // ✅ GET: Get project by Id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound("Project not found");

            var dto = new ProjectsDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                ImageUrl = project.ImageUrl,
                UserProfileId = project.UserProfileId
            };

            return Ok(dto);
        }

        // ✅ GET: Get projects by UserProfileId
        [HttpGet("GetByProfile/{userProfileId}")]
        public async Task<IActionResult> GetByProfile(int userProfileId)
        {
            var projects = await _context.Projects
                .Where(p => p.UserProfileId == userProfileId)
                .Select(p => new ProjectsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    UserProfileId = p.UserProfileId
                }).ToListAsync();

            return Ok(projects);
        }

        // ✅ POST: Add project
        [HttpPost]
        public async Task<IActionResult> Add(ProjectsDto dto)
        {
            var project = new Projects
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                UserProfileId = dto.UserProfileId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            dto.Id = project.Id;

            return Ok(dto);
        }

        // ✅ PATCH: Edit project
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, ProjectsDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound("Project not found");

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.ImageUrl = dto.ImageUrl;
            project.UserProfileId = dto.UserProfileId;

            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        // ✅ DELETE: Delete project
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound("Project not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
