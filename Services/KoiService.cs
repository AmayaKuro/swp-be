using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;

namespace swp_be.Services
{
    public class KoiService
    {
        // Create a service class for Koi in repository + service pattern
        private readonly KoiRepository koiRepository;

        public KoiService(ApplicationDBContext _context)
        {
            this.koiRepository= new KoiRepository(_context);
        }

        public async Task<IEnumerable<Koi>> GetKois()
        {
            return await koiRepository.GetAllAsync();
        }
    }
}
