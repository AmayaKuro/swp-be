using swp_be.Data.Repositories;
using swp_be.Data;
using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace swp_be.Services
{
    public class ConsignmentService
    {
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly ConsignmentRepository consignmentRepository;
        public ConsignmentService(ApplicationDBContext context)
        {
            this._context = context;
            this.unitOfWork= new UnitOfWork(context);
            this.consignmentRepository= new ConsignmentRepository(context);
        }
        public async Task<List<Consignment>> GetConsignment()
        {
            return await consignmentRepository.GetConsignment();
            ;
        }
        public async Task<Consignment> GetById(int id)
        {
            return await consignmentRepository.GetById(id);  // Query by ID
        }
        public async Task<bool> DeleteConsignment(Consignment consignment)
        {
            _context.Consignments.Remove(consignment);
            _context.SaveChanges();
            return true; // Return a boolean indicating success
        }

        public async Task<List<Consignment>> SearchConsignments(
           int? customerID = null,
           ConsignmentType? type = null,
           ConsigmentStatus? status = null,
           decimal? minFosterPrice = null,
           decimal? maxFosterPrice = null)
        {
            // Start the query with all consignments
            var query = _context.Consignments
                .Include(c => c.Customer)
                .ThenInclude(c => c.User)   // Include related User from Customer
                .AsQueryable();

            // Apply filters conditionally
            if (customerID.HasValue)
            {
                query = query.Where(c => c.CustomerID == customerID.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (minFosterPrice.HasValue)
            {
                query = query.Where(c => c.FosterPrice >= minFosterPrice.Value);
            }

            if (maxFosterPrice.HasValue)
            {
                query = query.Where(c => c.FosterPrice <= maxFosterPrice.Value);
            }

            // Execute the query asynchronously and return the results
            return await query.ToListAsync();
        }
    

    }
}
