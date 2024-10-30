using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using swp_be.data.Repositories;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>
    {
       
            public BlogRepository(ApplicationDBContext context) : base(context)
            {


            }
        public List<Blog> GetBlogsByUser(int userId)
        {
            return _context.Blogs
                .Where(blog => blog.UserID == userId)
                .ToList();
        }
        // return _context.Tokens
        //     .Where(userToken => userToken.RefreshToken == token)
        //   .Include(token => token.User)
        // .SingleOrDefault();
    }
}
