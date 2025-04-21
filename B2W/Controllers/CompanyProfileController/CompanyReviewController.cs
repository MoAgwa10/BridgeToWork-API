using B2W.Models.Dto.CompanyProfileDto;
using B2W.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers.CompanyProfileController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Reviews
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _context.CompanyReviews
                .Include(r => r.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Select(r => new
                {
                    r.Id,
                    r.Message,
                    r.Rating,
                    r.CompanyProfileId,
                    r.UserProfileId,
                    ReviewerName = r.UserProfile.FirstName + " " + r.UserProfile.LastName,
                    ReviewerImageUrl = r.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ Get Review by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _context.CompanyReviews
                .Include(r => r.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.Message,
                    r.Rating,
                    r.CompanyProfileId,
                    r.UserProfileId,
                    ReviewerName = r.UserProfile.FirstName + " " + r.UserProfile.LastName,
                    ReviewerImageUrl = r.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // ✅ Get Reviews by CompanyProfileId
        [HttpGet("company/{companyProfileId}")]
        public async Task<IActionResult> GetReviewsByCompanyId(int companyProfileId)
        {
            var reviews = await _context.CompanyReviews
                .Include(r => r.UserProfile)
                    .ThenInclude(p => p.UserProfilePictures)
                .Where(r => r.CompanyProfileId == companyProfileId)
                .Select(r => new
                {
                    r.Id,
                    r.Message,
                    r.Rating,
                    r.CompanyProfileId,
                    r.UserProfileId,
                    ReviewerName = r.UserProfile.FirstName + " " + r.UserProfile.LastName,
                    ReviewerImageUrl = r.UserProfile.UserProfilePictures
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => $"data:image/jpeg;base64,{Convert.ToBase64String(p.Image)}")
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ Add Review
        [HttpPost]
        [Authorize(Roles = "User")] // فقط المستخدمين الحقيقيين يعملوا ريفيو
        public async Task<IActionResult> AddReview([FromBody] CompanyReview review)
        {
            _context.CompanyReviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }

        // ✅ Update Review
        [HttpPatch("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] CompanyReview updatedReview)
        {
            var review = await _context.CompanyReviews.FindAsync(id);
            if (review == null)
                return NotFound();

            review.Message = updatedReview.Message;
            review.Rating = updatedReview.Rating;

            await _context.SaveChangesAsync();
            return Ok(review);
        }

        // ✅ Delete Review
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.CompanyReviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.CompanyReviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}