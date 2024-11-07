using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Models;

namespace swp_be.Data.Repositories
{
    public class ConsignmentRepository : GenericRepository<Consignment>
    {   
        public ConsignmentRepository(ApplicationDBContext _context) : base(_context)
        {
        }

        public async Task<List<Consignment>> GetConsignment()
        {
            return await _context.Consignments
                .Include(c=>c.ConsignmentPriceList)
                .Include(c => c.Customer)                    // Include the Customer entity
                .ThenInclude(c => c.User)                    // Include the User entity from the Customer
                .Include(c => c.ConsignmentKois)             // Include related ConsignmentKois
                .ThenInclude(c=>c.AddOn)                     // include addOn  
                .ToListAsync();
        }
        public async Task<List<Consignment>> GetConsignmentByCustomerId(int customerId)
        {
            return await _context.Consignments
                .Include(c => c.ConsignmentPriceList)
                .Include(c => c.Customer)                    // Include the Customer entity
                .ThenInclude(c => c.User)                    // Include the User entity from the Customer
                .Include(c => c.ConsignmentKois)             // Include related ConsignmentKois
                .ThenInclude(c => c.AddOn)                   // Include AddOn 
                .Where(c => c.Customer.UserID == customerId) // Filter by CustomerID
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
       
    }
}
