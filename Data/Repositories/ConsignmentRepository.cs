using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Models;

namespace swp_be.Data.Repositories
{
    public class ConsignmentRepository : GenericRepository<Consignment>
    {   
        private readonly TransactionRepository transactionRepository;
        private readonly UnitOfWork unitOfWork;
        public ConsignmentRepository(ApplicationDBContext _context) : base(_context)
        {
            transactionRepository = new TransactionRepository(_context);  
            unitOfWork=new UnitOfWork(_context);
        }
        public async Task<List<Consignment>> GetConsignment()
        {
            return await _context.Consignments
                .Include(c => c.Customer)                    // Include the Customer entity
                .ThenInclude(c => c.User)                    // Include the User entity from the Customer
                .Include(c => c.ConsignmentKois)             // Include related ConsignmentKois
                .ToListAsync();
        }
        public async Task<Consignment> GetById(int id)
        {
            return await _context.Consignments
                .Include(c => c.Customer)                      // Include related Customer
                .ThenInclude(c => c.User)                      // Include related User from Customer
                .Include(c => c.ConsignmentKois)               // Include related ConsignmentKois
                .FirstOrDefaultAsync(c => c.ConsignmentID == id);  // Query by ID
        }
        public async Task<bool> DeleteConsignment(Consignment consignment)
        {
            var transactions = await _context.Transactions
                                .Where(t => t.ConsignmentID == consignment.ConsignmentID)
                                .ToListAsync();

            foreach (var transaction in transactions)
            {
                transactionRepository.Remove(transaction);
            }

            unitOfWork.ConsignmentRepository.Remove(consignment);
            unitOfWork.Save();

            return true; // Return a boolean indicating success
        }
    }
}
