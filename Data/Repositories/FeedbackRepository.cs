using swp_be.Data;
using swp_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using swp_be.data.Repositories;

namespace swp_be.Data.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>
    {
        public FeedbackRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksWithUserAsync()
        {
            return await _context.Feedbacks
                                 .Include(f => f.Customer) 
                                 .ThenInclude(c => c.User) 
                                 .Include(f => f.Order)    
                                 .ToListAsync();
        }

        public async Task<Feedback> GetFeedbackWithUserByIdAsync(int id)
        {
            return await _context.Feedbacks
                                 .Include(f => f.Customer) 
                                 .ThenInclude(c => c.User) 
                                 .Include(f => f.Order)    
                                 .FirstOrDefaultAsync(f => f.FeedbackID == id);
        }

        public void UpdatePartial(Feedback feedback)
        {
            // Chỉ đánh dấu các trường được sửa đổi
            _context.Feedbacks.Attach(feedback);
            _context.Entry(feedback).Property(f => f.Rating).IsModified = true;
            _context.Entry(feedback).Property(f => f.Comment).IsModified = true;
        }

    }
}
