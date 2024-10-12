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
    }
}
