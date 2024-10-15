using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

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
            return Ok(await blogService.GetBlogs());
            //return await _context.Kois.Take(10).ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await blogService.UpdatBlog(blog);
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

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog (Blog blog)
        {
            await blogService.CreatBlog(blog);

            return CreatedAtAction("GetKoi", new { id =blog.BlogId }, blog);
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
