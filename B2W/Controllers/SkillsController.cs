using B2W.Dto;
using B2W.Models.Authentication;
using B2W.Models.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B2W.Models;
using B2W.Models.Dto;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. جلب جميع المهارات لكل المستخدمين
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillsDto>>> GetSkills()
        {
            var skills = await _context.Skills
                                       .Select(s => new SkillsDto
                                       {
                                           Id = s.Id,
                                           Name = s.Name,
                                           ApplicationUserId = s.ApplicationUserId
                                       })
                                       .ToListAsync();

            return Ok(skills);
        }

        // ✅ 2. جلب جميع المهارات لمستخدم معين عبر `ApplicationUserId`
        [HttpGet("ByUser/{ApplicationUserId}")]
        public async Task<ActionResult<IEnumerable<SkillsDto>>> GetSkillsByUser(string ApplicationUserId)
        {
            var skills = await _context.Skills
                                       .Where(s => s.ApplicationUserId == ApplicationUserId)
                                       .Select(s => new SkillsDto
                                       {
                                           Id = s.Id,
                                           Name = s.Name,
                                           ApplicationUserId = s.ApplicationUserId
                                       })
                                       .ToListAsync();

            if (!skills.Any())
                return NotFound("No skills found for this user.");

            return Ok(skills);
        }

        // ✅ 3. جلب مهارة واحدة بناءً على `id`
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillsDto>> GetSkill(int id)
        {
            var skill = await _context.Skills
                                      .Where(s => s.Id == id)
                                      .Select(s => new SkillsDto
                                      {
                                          Id = s.Id,
                                          Name = s.Name,
                                          ApplicationUserId = s.ApplicationUserId
                                      })
                                      .FirstOrDefaultAsync();

            if (skill == null)
                return NotFound("Skill not found.");

            return Ok(skill);
        }

        // ✅ 4. إضافة مهارة جديدة باستخدام `ApplicationUserId` الممرر من الـ Frontend
        [HttpPost]
        public async Task<ActionResult<SkillsDto>> PostSkill(SkillsDto skillDto)
        {
            var skill = new Skills
            {
                Name = skillDto.Name,
                ApplicationUserId = skillDto.ApplicationUserId // المستخدم يرسل `ApplicationUserId` يدويًا
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, new SkillsDto
            {
                Id = skill.Id,
                Name = skill.Name,
                ApplicationUserId = skill.ApplicationUserId
            });
        }

        // ✅ 5. تعديل مهارة بناءً على `id`
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(int id, SkillsDto skillDto)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == skillDto.ApplicationUserId);

            if (skill == null)
                return NotFound("Skill not found or you don't have permission to edit it.");

            skill.Name = skillDto.Name;

            _context.Entry(skill).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ 6. حذف مهارة بناءً على `id`
        [HttpDelete("{id}/{ApplicationUserId}")]
        public async Task<IActionResult> DeleteSkill(int id, string ApplicationUserId)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == ApplicationUserId);

            if (skill == null)
                return NotFound("Skill not found or you don't have permission to delete it.");

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return NoContent();


        }
    }
}
