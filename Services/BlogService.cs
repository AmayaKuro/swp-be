using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await unitOfWork.BlogRepository.GetAllAsync();
        }
        public async Task<Blog> CreatBlog(Blog blog)
        {
            unitOfWork.BlogRepository.Create(blog);
            unitOfWork.Save();
            return blog;
        }
        public async Task<Blog> UpdatBlog(Blog blog)
        {
            unitOfWork.BlogRepository.Update(blog);
            unitOfWork.Save();

            return blog;
        }
        public async Task<Blog> DeleteBlog(Blog blog)
        {
            unitOfWork.BlogRepository.Remove(blog);
            unitOfWork.Save();

            return blog;
        }

    }
}
