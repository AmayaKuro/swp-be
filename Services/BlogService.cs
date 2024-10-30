using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;

namespace swp_be.Services
{
 public class BlogService
    {
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly BlogRepository blogRepository;
        public BlogService(ApplicationDBContext context) 
        {
        this._context = context;
        this.unitOfWork=new UnitOfWork(context);
            this.blogRepository = new BlogRepository(context);
        }
        public async Task<List<Blog>> GetBlogs()
        {
            return await _context.Blogs.ToListAsync();
                
        }
        public async Task<Blog> GetById(int id)
        {
            return blogRepository.GetById(id);
              
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
