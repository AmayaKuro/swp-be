﻿using swp_be.Data;
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
            return await unitOfWork.FosterKoiRepository.GetAllAsync();
        }

        
        public async Task<ConsignmentKoi> DeleteKoi(ConsignmentKoi koi)
        {
            unitOfWork.FosterKoiRepository.Remove(koi);
            unitOfWork.Save();
            return koi;
        }

    }
}
