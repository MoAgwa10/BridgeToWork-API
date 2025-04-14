using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Usersearch")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("يجب إدخال نص البحث");
            }

            var users = await _context.Users
                .Where(u => u.UserName.Contains(query) ||
                            u.FirstName.Contains(query) ||
                            u.LastName.Contains(query) ||
                            u.Email.Contains(query))
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound("User Not Found");
            }

            return Ok(users);
        }





        [HttpGet("Jopsearch")]
        public async Task<IActionResult> SearchJops([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("يجب إدخال نص البحث");
            }

            var users = await _context.Jops
                .Where(u => u.Title.Contains(query) ||
                            u.Description.Contains(query) ||
                            u.WorkingModel.Contains(query) ||
                            u.JopType.Contains(query) ||
                            u.Requirments.Contains(query) ||
                            u.AboutCompany.Contains(query))

                .Select(u => new
                {
                    u.JopId,
                    u.Description,
                    u.Requirments,
                    u.AboutCompany,
                    u.JopType,
                    u.Title
                })
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound("jop Not Found");
            }

            return Ok(users);
        }
    }
}
