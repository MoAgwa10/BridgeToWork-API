using B2W.Models.Dto;
using B2W.Models.UserProfilePic;
using B2W.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilePicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfilePicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add User Profile Picture
        [HttpPost("AddUserProfilePicture")]
        public async Task<IActionResult> AddUserProfilePicture([FromForm] UserProfilePictureAddDto userProfilePictureDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // حفظ الصورة إذا تم تحميلها
            byte[] imageBytes = null;
            if (userProfilePictureDto.Image != null)
            {
                imageBytes = await ConvertImageToByteArrayAsync(userProfilePictureDto.Image);
            }

            var userProfilePicture = new UserProfilePicture
            {
                UserId = userProfilePictureDto.UserId,
                Image = imageBytes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.userProfilePictures.Add(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok(userProfilePicture);
        }

        // Get User Profile Picture by UserProfilePictureId
        [HttpGet("GetUserProfilePictureById/{id}")]
        public async Task<IActionResult> GetUserProfilePictureById(int id)
        {
            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
            {
                return NotFound("User Profile Picture Not Found.");
            }

            return File(userProfilePicture.Image, "image/jpeg"); // أو أي نوع صورة آخر
        }

        // Get User Profile Picture by UserId
        [HttpGet("GetUserProfilePictureByUserId/{userId}")]
        public async Task<IActionResult> GetUserProfilePictureByUserId(string userId)
        {
            var userProfilePicture = await _context.userProfilePictures
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (userProfilePicture == null)
            {
                return NotFound("User Profile Picture Not Found.");
            }

            return File(userProfilePicture.Image, "image/jpeg"); // أو أي نوع صورة آخر
        }

        // Edit User Profile Picture
        [HttpPut("EditUserProfilePicture/{id}")]
        public async Task<IActionResult> EditUserProfilePicture(int id, [FromForm] UserProfilePictureEditDto userProfilePictureDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
            {
                return NotFound("User Profile Picture Not Found.");
            }

            // تحديث الصورة إذا تم تحميل صورة جديدة
            if (userProfilePictureDto.Image != null)
            {
                userProfilePicture.Image = await ConvertImageToByteArrayAsync(userProfilePictureDto.Image);
                userProfilePicture.UpdatedAt = DateTime.UtcNow;
            }

            _context.userProfilePictures.Update(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok(userProfilePicture);
        }

        // Delete User Profile Picture
        [HttpDelete("DeleteUserProfilePicture/{id}")]
        public async Task<IActionResult> DeleteUserProfilePicture(int id)
        {
            var userProfilePicture = await _context.userProfilePictures.FindAsync(id);
            if (userProfilePicture == null)
            {
                return NotFound("User Profile Picture Not Found.");
            }

            _context.userProfilePictures.Remove(userProfilePicture);
            await _context.SaveChangesAsync();

            return Ok("User Profile Picture Deleted.");
        }

        // Convert Image to Byte Array
        private async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
