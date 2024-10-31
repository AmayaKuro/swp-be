using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Models;

namespace swp_be.Services
{
    public class ConsignmentKoiService
    {
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;

        public ConsignmentKoiService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
        }

        public async Task<IEnumerable<ConsignmentKoi>> GetConsignmentKois()
        {
            return await _context.ConsignmentKois
                           .Include(koi => koi.Consignment)
                           .Include(c => c.AddOn)
                           .ToListAsync();
        }

        public async Task<ConsignmentKoi> GetConsignmentKoi(int id)
        {
            return await unitOfWork.ConsignmentKoiRepository.GetByIdAsync(id);
        }

        public async Task<ConsignmentKoi> GetById(int id)
        {
            return await _context.ConsignmentKois.Include(koi => koi.Consignment)
                                                 .Include(koi => koi.AddOn)
                                                 .FirstOrDefaultAsync(koi => koi.ConsignmentKoiID == id);
        }
        public async Task<List<ConsignmentKoi>> GetConsignmentKoisByUser(int userId)
        {
            return await unitOfWork.ConsignmentKoiRepository.GetConsignmentKoisByUserId(userId);
        }

        public async Task<ConsignmentKoi> DeleteKoi(ConsignmentKoi koi)
        {
            unitOfWork.ConsignmentKoiRepository.Remove(koi);
            unitOfWork.Save();
            return koi;
        }

    }
}
