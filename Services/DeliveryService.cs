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
        private readonly DeliveryRepository _repository;

        public DeliveryService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
            _repository = new DeliveryRepository(_context);
        }

        public async Task<IEnumerable<Delivery>> GetDeliveries()
        {
            return await _repository.GetDeliveries();

        }

        public Delivery GetDeliveryById(int deliveryId)
        {

            return _repository.GetDeliveryById(deliveryId);       
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
