using swp_be.data.Repositories;
using swp_be.Models;
using YourNamespace.Models;

namespace swp_be.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>
    {
       
            public BlogRepository(ApplicationDBContext context) : base(context)
            {


            }

        
    }
}
