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

        public KoiService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
        }

        public async Task<IEnumerable<Koi>> GetKois()
        {
            return await unitOfWork.KoiRepository.GetAllAsync();
        }

        public async Task<Koi> CreateKoi(Koi koi)
        {
            unitOfWork.KoiRepository.Create(koi);
            unitOfWork.Save();
            return koi;
        }
    }
}
