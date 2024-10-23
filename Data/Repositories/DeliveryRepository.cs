using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Models;


namespace swp_be.Data.Repositories
{
    public class DeliveryRepository : GenericRepository<Delivery>
    {
        public DeliveryRepository(ApplicationDBContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Delivery>> GetDeliveries()
        {
            return _context.Deliveries
                    .Include(d => d.Order)
                    .Include(d => d.Customer);

        }
        public Delivery GetDeliveryById(int deliveryId)
        {
            return _context.Deliveries
                       .Include(d => d.Order)    // Eager loading the Order entity
                       .Include(d => d.Customer) // Eager loading the Customer entity
                       .FirstOrDefault(d => d.DeliveryID == deliveryId); // Fetch delivery by ID
        }
    }
}