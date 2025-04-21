using B2W.Models;
using B2W.Models.Dto;
using B2W.Models.Userpost;
using B2W.Models.UserRecations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserReactionController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserReaction([FromBody] UserReactionAddDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check Validation 
            if (dto.PostId <= 0 || dto.UserId is null)
            {
                return BadRequest("PostId or UserId unvalid");
            }

            // Step 1: Check if the user has already reacted to this post
            var existingReaction = await _context.userReactions
                                                 .FirstOrDefaultAsync(r => r.UserId == dto.UserId && r.PostId == dto.PostId);

            // Step 2: If a previous reaction exists, remove it
            if (existingReaction != null)
            {
                _context.userReactions.Remove(existingReaction);
                // You could also reduce the previous reaction count here, if necessary
            }

            // Step 3: Create a new reaction
            var userReaction = new UserReaction
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                ReactionTypeId = dto.ReactionTypeId,
                CreatedAt = DateTime.Now
            };


            try
            {
                _context.userReactions.Add(userReaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserReactionById), new { id = userReaction.ReactionId }, userReaction);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.InnerException?.Message}");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserReactionById(int id, [FromBody] UserReactionEditDto dto)
        {

            // Retrieve the UserReaction by ID
            var UserReaction = await _context.userReactions.FindAsync(id);
            if (UserReaction == null)
            {
                return NotFound();
            }

            // Update UserReaction properties
            UserReaction.ReactionTypeId = dto.ReactionTypeId;

            // UpdateAt will update date in database (after trigger)
            try
            {
                _context.userReactions.Update(UserReaction);
                await _context.SaveChangesAsync();
                return Ok("User reaction Updated successfully.");

            }
            catch (DbUpdateException ex)
            {
                // Log the exception using a logging framework
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }

            return NoContent(); // 204 status code mean the operation is done
        }

        [HttpDelete("{Reaction_id}")]
        public async Task<IActionResult> DeleteUserReactionById(int Reaction_id)
        {
            // Find the post by ID
            var UserReaction = await _context.userReactions.FindAsync(Reaction_id);
            if (UserReaction == null)
            {
                return NotFound();
            }

            try
            {
                // Remove the post
                _context.userReactions.Remove(UserReaction);

                // Save changes asynchronously
                await _context.SaveChangesAsync();
                return Ok("User reaction deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

            }
        }

        [HttpDelete("user/{userId}/post/{postId}")]
        public async Task<IActionResult> DeleteUserReactionByUserId_PostId(string userId, int postId)
        {
            // Find the post by ID
            var UserReaction = await _context.userReactions.FirstOrDefaultAsync((u) => u.UserId == userId && u.PostId == postId);
            if (UserReaction == null)
            {
                return NotFound();
            }
            try
            {


                // Remove the post
                _context.userReactions.Remove(UserReaction);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                // Return NoContent (204) as a standard response for deletion
                return Ok("User reaction deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReaction>> GetUserReactionById(int id)
        {
            try
            {
                var UserReaction = await _context.userReactions.FindAsync(id);
                if (UserReaction == null)
                {
                    return NotFound();
                }

                return UserReaction;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

            }
        }


        [HttpGet("posts/{postId}")]
        public async Task<ActionResult<IEnumerable<UserReactionAddDto>>> GetUserReactionByPostId(int postId)
        {

            // Retrieve posts for the specified user
            var UserReactions = await _context.userReactions
                                          .Where(reaction => reaction.PostId == postId)
                                          .ToListAsync();

            // If no posts found, return a 404 NotFound
            if (UserReactions == null || !UserReactions.Any())
            {
                return NotFound($"No posts with postId : {postId}");
            }

            try
            {
                // Map posts to a DTO if needed
                var reactions = UserReactions.Select(reaction => new UserReactionAddDto
                {
                    UserId = reaction.UserId,
                    PostId = reaction.PostId.HasValue ? reaction.PostId.Value : throw new InvalidOperationException("PostId is null"),
                    ReactionTypeId = reaction.ReactionTypeId
                }).ToList();

                // Return the list of user posts
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");

            }
        }

        [HttpGet("users/{userd}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserReactionByUserId(string userd)
        {
            // Retrieve posts for the specified user
            var UserReactions = await _context.userReactions
                                          .Where(reaction => reaction.UserId == userd)
                                          .ToListAsync();

            // If no posts found, return a 404 NotFound
            if (UserReactions == null || !UserReactions.Any())
            {
                return NotFound($"No posts with postId : {userd}");
            }

            try
            {

                // Map posts to a DTO if needed
                var reactions = UserReactions.Select(reaction => new UserReactionAddDto
                {
                    UserId = reaction.UserId,
                    PostId = reaction.PostId.HasValue ? reaction.PostId.Value : throw new InvalidOperationException("PostId is null"),
                    ReactionTypeId = reaction.ReactionTypeId
                }).ToList();

                // Return the list of user posts
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
