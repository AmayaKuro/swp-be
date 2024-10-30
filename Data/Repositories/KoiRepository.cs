using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class KoiRepository : GenericRepository<Koi>
    {
        public KoiRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<List<object>> GetAvailableKoisAsync()
        {
            var availableKois = await _context.Kois
                .Where(k => k.Status == KoiStatus.Available)
                .ToListAsync();

            var availableConsignmentKois = await _context.ConsignmentKois
                .Include(ck => ck.Consignment)
                .Where(ck => ck.Consignment.Type == ConsignmentType.Sell && ck.Consignment.Status == ConsignmentStatus.available)
                .ToListAsync();

            var result = new List<object>();
            result.AddRange(availableKois);
            result.AddRange(availableConsignmentKois);

            return result;
        }

        public async Task<Koi> GetByIdAsync(int id)
        {
            return await _context.Kois
                .Include(k => k.AddOn)
                .FirstOrDefaultAsync(k => k.KoiID == id);
        }

        public async Task<Koi[]> GetAllAsync()
        {
            return await _context.Kois
                .Include(k => k.AddOn)
                .ToArrayAsync();
        }
    }
}
