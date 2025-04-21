using B2W.Models;
using B2W.Models.Dto;
using B2W.Models.Userpost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Add Post
        [Authorize(Roles = "User")]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromForm] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // حفظ الصورة إذا تم تحميلها
            string imagePath = null;
            if (postDto.Image != null)
            {
                imagePath = await SaveImageAsync(postDto.Image);
            }

            var post = new Post
            {
                UserId = postDto.UserId,
                Content = postDto.Content,
                CreatedAt = DateTime.UtcNow,
                Image = imagePath
            };

            _context.posts.Add(post);
            await _context.SaveChangesAsync();

            // إرجاع المنشور الذي تم إنشاؤه
            return Ok(post);
        }

        //  لحفظ الصورة على السيرفر
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            // إنشاء المسار إذا لم يكن موجودًا
            var imagesPath = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            // إنشاء اسم فريد للصورة
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(imagesPath, fileName);

            // حفظ الصورة في المجلد المحدد
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // إرجاع مسار الصورة
            return $"/images/{fileName}";
        }






        //Get All posts
        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.posts.ToListAsync();

            // إرجاع روابط الصور الكاملة
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var post in posts)
            {
                if (!string.IsNullOrEmpty(post.Image))
                {
                    post.Image = $"{baseUrl}{post.Image}";
                }
            }

            return Ok(posts);
        }



        [HttpGet("GetPost/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post Is Not Found.");
            }

            // إرجاع رابط الصورة الكامل
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            if (!string.IsNullOrEmpty(post.Image))
            {
                post.Image = $"{baseUrl}{post.Image}";
            }

            return Ok(post);
        }







        //Edit post
        [HttpPut("EditPost/{id}")]
        public async Task<IActionResult> EditPost(int id, [FromForm] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // البحث عن المنشور الموجود
            var post = await _context.posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post Is Not Found.");
            }

            // تحديث المحتوى
            post.Content = updatePostDto.Content;
            post.UpdatedAt = DateTime.UtcNow;

            // تحديث الصورة إذا تم تحميل صورة جديدة
            if (updatePostDto.Image != null)
            {
                // حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(post.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot", post.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // حفظ الصورة الجديدة
                post.Image = await SaveImageAsync(updatePostDto.Image);
            }

            _context.posts.Update(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }




        //Delete Post
        [HttpDelete("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post Is Not Found");
            }

            // حذف الصورة المرتبطة إذا كانت موجودة
            if (!string.IsNullOrEmpty(post.Image))
            {
                var imagePath = Path.Combine("wwwroot", post.Image.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok("Post Is Deleted");
        }

    }
}
