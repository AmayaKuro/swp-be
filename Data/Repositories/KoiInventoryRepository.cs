using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Models;

namespace swp_be.Data.Repositories
{
    public class KoiInventoryRepository : GenericRepository<KoiInventory>
    {
        public KoiInventoryRepository(ApplicationDBContext context) : base(context)
        {

        }
        public async Task<List<KoiInventory>> GetIKois()
        {
            return await _context.KoiInventory
                    .Include(c => c.AddOn)
                    .Include(c=>c.Customer)
                    .ToListAsync();
        }

        public async Task<List<KoiInventory>> GetKoiInventoryByUserId(int userId)
        {
            return await _context.KoiInventory
                .Include(k => k.AddOn)
                .Where(k => k.CustomerID == userId)
                .ToListAsync();
        }
    }
}
