using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;

namespace swp_be.Services
{
    public class DeliveryService
    {
        // Create a service class for Koi in repository + service pattern
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;

        public DeliveryService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
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
                       .Include(d => d.Order)
                       .Include(d => d.Customer)
                       .FirstOrDefault(d => d.DeliveryID == deliveryId);

        }

        public async Task<Delivery> UpdateDelivery(Delivery delivery)
        {
            
            
            
            unitOfWork.DeliverRepository.Update(delivery);
            unitOfWork.Save();

            return delivery;
        }
        public async Task<Delivery> DeleteDelivery(Delivery delivery)
        {
            unitOfWork.DeliverRepository.Remove(delivery);
            unitOfWork.Save();

            return delivery;
        }

        
    }
}
