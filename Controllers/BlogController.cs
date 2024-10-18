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
            int userId = int.Parse(User.FindFirstValue("userID"));
            User user = userService.GetUserProfile(userId);

           

           return blogService.CreateBlog(blog, userId);
            
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
