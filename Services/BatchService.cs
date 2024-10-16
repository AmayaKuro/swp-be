using System.Collections.Generic;
using System.Threading.Tasks;
using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public class BatchService
    {

        private readonly UnitOfWork unitOfWork;

        public BatchService(ApplicationDBContext context)
        {
            this.unitOfWork = new UnitOfWork(context);
        }

        public async Task<IEnumerable<Batch>> GetBatches()
        {
            return await unitOfWork.BatchRepository.GetAllAsync();
        }

        public async Task<Batch> CreateBatch(Batch batch)
        {
            unitOfWork.BatchRepository.Create(batch);
            unitOfWork.Save();
            return batch;
        }

        public async Task<Batch> UpdateBatch(Batch batch)
        {
            unitOfWork.BatchRepository.Update(batch);
            unitOfWork.Save();
            return batch;
        }

        public async Task<bool> DeleteBatch(int id)
        {
            var batch = await unitOfWork.BatchRepository.GetByIdAsync(id);
            if (batch == null)
                return false;

            unitOfWork.BatchRepository.Remove(batch);
            unitOfWork.Save();
            return true;
        }

        public async Task<Batch> GetBatchById(int id)
        {
            return await unitOfWork.BatchRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Batch>> SearchBatches(string? name, string? species, decimal? minPrice, decimal? maxPrice)
        {
            var batches = await GetBatches();

            if (!string.IsNullOrEmpty(name))
            {
                batches = batches.Where(b => b.Name != null && b.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(species))
            {
                batches = batches.Where(b => b.Species != null && b.Species.Contains(species, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                batches = batches.Where(b => b.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                batches = batches.Where(b => b.Price <= maxPrice);
            }

            return batches;
        }
    }
}
