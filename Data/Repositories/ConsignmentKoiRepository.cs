using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace swp_be.Data.Repositories
{
    public class ConsignmentKoiRepository : GenericRepository<ConsignmentKoi>
    {
        public ConsignmentKoiRepository(ApplicationDBContext context) : base(context)
        {
        }

        public List<ConsignmentKoi> getConsignmentKois()
        {
            return _context.ConsignmentKois.Include(koi => koi.Consignment).ToList();
        }

        public ConsignmentKoi GetById(int id)
        {
            return _context.ConsignmentKois.Include(koi => koi.Consignment)
                                           .FirstOrDefault(koi => koi.ConsignmentID == id);
        }

        public async Task<List<ConsignmentKoi>> GetConsignmentKoisByUserId(int userId)
        {
            return await _context.ConsignmentKois
                .Include(koi => koi.Consignment)
                .ThenInclude(consignment => consignment.Customer)
                .Where(koi => koi.Consignment.Customer.UserID == userId)
                .ToListAsync();
        }
    }
}