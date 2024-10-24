using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace swp_be.Data.Repositories
{
    public class ConsigmentKoiRepository : GenericRepository<ConsignmentKoi>
    {
        public ConsigmentKoiRepository(ApplicationDBContext context) : base(context)
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
    }
}