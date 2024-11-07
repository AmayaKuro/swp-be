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
        }
        public async Task<List<Consignment>> GetConsignmentByCustomer(int id)
        {
            return await consignmentRepository.GetConsignmentByCustomerId(id);
        }

        public async Task<Consignment> GetById(int id)
        {
            return await consignmentRepository.GetById(id);  // Query by ID
        }

        public async Task<List<ConsignmentPriceList>> GetPriceList()
        {
            return await unitOfWork.ConsignmentPriceListRepository.GetAllAsync();
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
           ConsignmentStatus? status = null,
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

        public async Task<Consignment> UpdateConsignment(Consignment consignment)
        {
            try
            {
                consignmentRepository.Update(consignment);
                consignmentRepository.Save();
                return consignment;
            }
            catch (DbUpdateException ex)
            {
                // Optionally, log the exception or rethrow as needed
                throw new Exception("An error occurred while updating the consignment.", ex);
            }
        }

        public async Task<Consignment> CreateConsignment(Consignment consignment, ConsignmentKoi consignmentKoi)
        {
            _context.Consignments.Add(consignment);
            await _context.SaveChangesAsync();
            try
            {
                // Set the ConsignmentID in consignmentKoi after saving consignment
                consignmentKoi.ConsignmentID = consignment.ConsignmentID;

                // Add the consignmentKoi to the database
                _context.ConsignmentKois.Add(consignmentKoi);
              await  _context.SaveChangesAsync(); // Save the Koi as well

                return consignment;
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for debugging purposes if needed
                throw new Exception("Error creating consignment", ex);
            }
        }
    }
}
