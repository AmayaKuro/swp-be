using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;

namespace swp_be.Data.Repositories
{
    public class PromotionRepository : GenericRepository<Promotion>
    {
        public PromotionRepository(ApplicationDBContext context) : base(context)
        {
        }

        
    }
}
