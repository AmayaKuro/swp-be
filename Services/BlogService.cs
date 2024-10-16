using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Models;
using YourNamespace.Models;

namespace swp_be.Services
{
 public class BlogService
    {
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;
        public BlogService(ApplicationDBContext context) 
        {
        this._context = context;
        this.unitOfWork=new UnitOfWork(context);
        }
        public async Task<List<Blog>> GetBlogs()
        {
            return await _context.Blogs
                .Include(b => b.User) // Include the User navigation property
                .ToListAsync();
        }
        public async Task<Blog> GetById(int id)
        {
            return await _context.Blogs
                .Include(b => b.User) // Include the User navigation property
                .FirstOrDefaultAsync(b => b.BlogId == id); // Assuming 'Id' is the primary key
        }
       public async Task<Blog> CreateBlog(Blog blog, int userId)
{
    var user = await _context.Users.FindAsync(userId);
    if (user == null)
    {
        throw new InvalidOperationException($"User with ID {userId} not found.");
    }

    blog.User.UserID = userId;
    blog.User = null; // Ensure we're not trying to create a new user
    blog.CreateAt = DateTime.UtcNow;
    blog.UpdateAt = DateTime.UtcNow;

            unitOfWork.BlogRepository.Create(blog);
            unitOfWork.Save(); // Changed from SaveAsync to Save
            return blog;
        }
        public async Task<Blog> UpdateBlog(Blog blog)
        {   
            var existingBlog = await _context.Blogs.FindAsync(blog.BlogId); // Find the existing blog by ID
            if (existingBlog == null)
            {
                return null; // Return null if the blog does not exist
            }

            // Update properties of the existing blog
            existingBlog.Title = blog.Title; // Update Title
            existingBlog.UpdateAt = DateTime.UtcNow; // Update UpdateAt to current time
                                                     // Note: CreateAt should not be updated as it represents the creation time

            await _context.SaveChangesAsync(); // Save changes to the database
            return existingBlog; // Return the updated blog
        }
        public async Task<Blog> DeleteBlog(Blog blog)
        {
            unitOfWork.BlogRepository.Remove(blog);
            unitOfWork.Save();

            return blog;
        }

    }
}
