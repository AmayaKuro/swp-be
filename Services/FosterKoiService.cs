using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public class FosterKoiService
    {
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;

        public FosterKoiService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
        }

        public async Task<IEnumerable<FosterKoi>> GetFosterKois()
        {
            return await unitOfWork.FosterKoiRepository.GetAllAsync();
        }

        
        public async Task<FosterKoi> DeleteKoi(FosterKoi koi)
        {
            unitOfWork.FosterKoiRepository.Remove(koi);
            unitOfWork.Save();
            return koi;
        }

    }
}
