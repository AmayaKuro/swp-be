using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace swp_be.Data.Repositories
{
    public class PromotionRepository : GenericRepository<Promotion>
    {
        public PromotionRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByDateAsync(DateTime startDate, DateTime? endDate)
        {
            return await _context.Promotions
                .Where(p => (p.StartDate <= endDate && (p.EndDate >= startDate || p.EndDate == null)))
                .ToListAsync();
        }


    }
}
