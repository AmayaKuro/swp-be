using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly BlogService blogService;
        private readonly UserService userService;
        public BlogController(ApplicationDBContext context)
        {
            this._context = context;
            this.blogService = new BlogService(context);
            this.userService = new UserService(context);
        }
        [HttpGet]
        public async Task<ActionResult<Blog>> GetBlog()
        {
            var blogs = await blogService.GetBlogs();


            return Ok(blogs);
        }

        [Authorize("staff, admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            var blog = await blogService.GetById(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }
        [Authorize("staff, admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, Blog blog)
        {
            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            var existingBlog = await _context.Blogs.FindAsync(id);

            if (existingBlog == null)
            {
                return NotFound();
            }

            // Check if the User is null
            if (blog.User == null)
            {
                return BadRequest("User information is required to update a blog.");
            }

            existingBlog.Title = blog.Title;

            // Update other fields as necessary, but not UserId or CreatedAt

            try
            {
                await blogService.UpdateBlog(existingBlog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(blog);
        }
        [Authorize("staff, admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBlog(
    [FromQuery] string title,
    [FromQuery] string BlogSlug,
    [FromQuery] string Description,
    [FromQuery] string Content,
    [FromQuery] DateTime? createdAt = null,
    [FromQuery] DateTime? updatedAt = null)
        {
            int userID = int.Parse(User.FindFirstValue("userID"));
            // Validate title
            if (string.IsNullOrWhiteSpace(title) || title.Length > 255)
            {
                return BadRequest("Invalid title. It must be non-empty and less than 255 characters.");
            }

            // Validate user ID
            if (userID <= 0)
            {
                return BadRequest("Invalid User ID.");
            }

            // If createdAt is not provided, set it to current time
            var createDate = createdAt ?? DateTime.UtcNow;

            // If updatedAt is not provided, set it to current time
            var updateDate = updatedAt ?? DateTime.UtcNow;

            // Create a new blog object
            var blog = new Blog
            {
                Title = title,
                BlogSlug = BlogSlug,
                Description = Description,
                Content = Content,
                CreateAt = createDate,
                UpdateAt = updateDate,
                UserID = userID
            };

            // Add the new blog to the context
            _context.Blogs.Add(blog);

            try
            {
                // Save changes asynchronously
                await _context.SaveChangesAsync();

                // Return 201 Created with the new blog's ID
                return CreatedAtAction(nameof(CreateBlog), new { id = blog.BlogId }, blog);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Koi/5
        [Authorize("staff, admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            await blogService.DeleteBlog(blog);

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
