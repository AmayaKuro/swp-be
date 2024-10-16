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
        public BlogController(ApplicationDBContext context)
        {
            this._context = context;
            this.blogService = new BlogService(context);
        }
        [HttpGet]
        public async Task<ActionResult<Blog>> GetBlog()
        {
            var blogs = await blogService.GetBlogs();
      

            return Ok(blogs);
        }


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
       
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog(Blog blog)
        {
            // Check if the User is null
            if (blog.User == null)
            {
                return BadRequest("User information is required to create a blog.");
            }

            // Extract UserId from the authenticated user's claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("User ID is not valid.");
            }

            // Optionally, validate other properties of the blog here

            var createdBlog = await blogService.CreateBlog(blog, userId); // Pass both blog and userId

            return CreatedAtAction(nameof(GetBlog), new { id = createdBlog.BlogId }, createdBlog);
        }
        // DELETE: api/Koi/5

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
