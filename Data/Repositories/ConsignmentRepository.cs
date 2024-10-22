using swp_be.data.Repositories;
using swp_be.Models;

namespace swp_be.Data.Repositories
{
    public class ConsignmentRepository : GenericRepository<Consignment>
    {
        public ConsignmentRepository(ApplicationDBContext _context) : base(_context)
        {
        }
    }
}
