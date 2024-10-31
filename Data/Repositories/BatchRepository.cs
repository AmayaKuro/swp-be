using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;

namespace swp_be.Data.Repositories
{
    public class BatchRepository : GenericRepository<Batch>
    {
        public BatchRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Batch>> GetAvailableBatches()
        {
            var availableBatches = await GetAllAsync();
            return availableBatches.Where(b => b.RemainBatch > 0).ToList();
        }


    }
}