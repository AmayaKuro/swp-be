﻿using Microsoft.EntityFrameworkCore;
using swp_be.Models;
using swp_be.Data;
using swp_be.Data.Repositories;
using System.Net;

namespace swp_be.Services
{
    public class DeliveryService
    {
        // Create a service class for Koi in repository + service pattern
        private ApplicationDBContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly DeliveryRepository _repository;
        private readonly OrderService orderService;
        private readonly UserService userService;

        public DeliveryService(ApplicationDBContext _context)
        {
            this._context = _context;
            this.unitOfWork = new UnitOfWork(_context);
            _repository = new DeliveryRepository(_context);
            orderService = new OrderService(_context);
            userService = new UserService(_context);
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
            if (delivery.Status == DeliveryStatus.Delivered)
            {
                // Finish the order when the delivery is completed
                orderService.FinishOrder(delivery.OrderID);
            }

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

        public Delivery CreateDeliveryFromOrder(Order order)
        {
            var customer = unitOfWork.CustomerRepository.GetById(order.CustomerID.Value);
            var user = unitOfWork.UserRepository.GetById(customer.UserID);

            Delivery delivery = new Delivery
            {
                OrderID = order.OrderID,
                Customer = customer,
                Status = DeliveryStatus.Delivering,
                StartDeliDay = DateTime.Now,
                Address = user.Address   
            };

            //customer.LoyaltyPoints += 10;
            //userService.UpdateCustomer(customer);

            unitOfWork.DeliverRepository.Create(delivery);
            unitOfWork.Save();
            return delivery;
        }


        public Delivery CreateDeliveryFromOrder(Order order, string? address)
        {
            var user = unitOfWork.UserRepository.GetById(order.CustomerID.Value);
            var customer = unitOfWork.CustomerRepository.GetById(order.CustomerID.Value);
            Delivery delivery = new Delivery
            {
                OrderID = order.OrderID,
                Customer = customer,
                Status = DeliveryStatus.Delivering,
                // Set the start delivery day to the current day if no consignment after order, otherwise set to consignment end date
                StartDeliDay = (order.Consignment != null) ? order.Consignment.EndDate : DateTime.Now,
                // Set address if provided, otherwise use the user's address
                Address = (address == null || address == "") ? user.Address : address,
            };

            unitOfWork.DeliverRepository.Create(delivery);
            unitOfWork.Save();
            return delivery;
        }

    }
}
