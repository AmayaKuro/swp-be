using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;

namespace swp_be.Services
{
    public class KoiService
    {
        // Create a service class for Koi in repository + service pattern
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly KoiRepository repository;

        public KoiService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
            repository= new KoiRepository(_context);
        }

        public async Task<IEnumerable<Koi>> GetKois()
        {
            return await  repository.GetKois();
        }

        public async Task<List<object>> GetAvailableKoisAsync()
        {
            return await unitOfWork.KoiRepository.GetAvailableKoisAsync();
        }

        public async Task<Koi> GetKoi(int id)
        {
            return await unitOfWork.KoiRepository.GetByIdAsync(id);
        }


        public async Task<Koi> CreateKoi(Koi koi)
        {
            unitOfWork.KoiRepository.Create(koi);
            unitOfWork.Save();
            return koi;
        }
        public async Task<Koi> UpdateKoi(Koi koi)
        {
            unitOfWork.KoiRepository.Update(koi);
            unitOfWork.Save();

            return koi;
        }
           public async Task<Koi> DeleteKoi(Koi koi)
        {
            unitOfWork.KoiRepository.Remove(koi);
            unitOfWork.Save();

            return koi;
        }
    

    }
}
