using B2W.Dto.UserProfileDtos;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.UserProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MillStonesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MillStonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Get all
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.MillStones
                .Select(m => new MillStoneDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Company = m.Company,
                    Date = m.Date,
                    UserProfileId = m.UserProfileId
                }).ToListAsync();

            return Ok(data);
        }

        // ✅ GET: Get by id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.MillStones.FindAsync(id);
            if (item == null) return NotFound();

            var dto = new MillStoneDto
            {
                Id = item.Id,
                Title = item.Title,
                Company = item.Company,
                Date = item.Date,
                UserProfileId = item.UserProfileId
            };

            return Ok(dto);
        }

        // ✅ GET: Get by UserProfileId
        [HttpGet("GetByProfile/{userProfileId}")]
        public async Task<IActionResult> GetByProfile(int userProfileId)
        {
            var data = await _context.MillStones
                .Where(m => m.UserProfileId == userProfileId)
                .Select(m => new MillStoneDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Company = m.Company,
                    Date = m.Date,
                    UserProfileId = m.UserProfileId
                }).ToListAsync();

            return Ok(data);
        }

        // ✅ POST: Add
        [HttpPost]
        public async Task<IActionResult> Add(MillStoneDto dto)
        {
            var entity = new MillStones
            {
                Title = dto.Title,
                Company = dto.Company,
                Date = dto.Date,
                UserProfileId = dto.UserProfileId
            };

            _context.MillStones.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return Ok(dto);
        }

        // ✅ PATCH: Update
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, MillStoneDto dto)
        {
            var item = await _context.MillStones.FindAsync(id);
            if (item == null) return NotFound();

            item.Title = dto.Title;
            item.Company = dto.Company;
            item.Date = dto.Date;
            item.UserProfileId = dto.UserProfileId;

            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.MillStones.FindAsync(id);
            if (item == null) return NotFound();

            _context.MillStones.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
