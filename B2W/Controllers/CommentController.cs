using B2W.Models;
using B2W.Models.Dto;
using B2W.Models.UserComment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }



        //Add comment 
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentAddDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(dto.CommentText) || string.IsNullOrEmpty(dto.UserId) || dto.PostId <= 0)
            {
                return BadRequest("Invalid content or user ID.");
            }

            var comment = new Comment
            {
                UserId = dto.UserId,
                CommentText = dto.CommentText,
                PostId = dto.PostId,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.CommentId }, comment);
        }



        //Edit Comment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommentById(int id, CommentEditDto dto)
        {
            var comment = await _context.comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            comment.CommentText = dto.CommentText;

            try
            {
                _context.comments.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }

            return NoContent();
        }



        //Delete Comment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentById(int id)
        {
            var comment = await _context.comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            try
            {
                _context.comments.Remove(comment);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        //Get All Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPostComments()
        {
            try
            {
                var comments = await _context.comments.ToListAsync();
                if (!comments.Any())
                {
                    return NoContent();
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        //Get comment by Comment Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            try
            {
                var comment = await _context.comments.FindAsync(id);
                if (comment == null)
                {
                    return NotFound();
                }
                return comment;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        //Get Post comments by Post Id

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPostCommentsByPostId(int postId)
        {
            try
            {
                var comments = await _context.comments.Where(c => c.PostId == postId).ToListAsync();
                if (!comments.Any())
                {
                    return NoContent();
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //Get User Comments By User Id
        [HttpGet("users/{userId}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUserCommentsByUserId(string userId)
        {
            var userComments = await _context.comments.Where(comment => comment.UserId == userId).ToListAsync();

            if (!userComments.Any())
            {
                return NotFound($"No comments found for UserId: {userId}");
            }

            try
            {
                return Ok(userComments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
