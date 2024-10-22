using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;

namespace swp_be.Data.Repositories
{
    public class FosterKoiRepository : GenericRepository<ConsignmentKoi>
    {
        public FosterKoiRepository(ApplicationDBContext context) : base(context)
        {
        }

    }
}