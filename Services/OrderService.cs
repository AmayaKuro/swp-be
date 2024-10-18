using Microsoft.EntityFrameworkCore;
using swp_be.Controllers;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;

namespace swp_be.Services
{
    /*
     * OrderService flow:
     * AddOrderDetails() to add order details
     * CreateOrder(customerID, promotionID) to create order
     */
    public class OrderService
    {
        public decimal TotalAmount { get; private set; }
        public Promotion promotion { get; set; }
        public List<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();


        private readonly ApplicationDBContext _context;
        private BatchRepository batchRepository;
        private KoiRepository koiRepository;
        private GenericRepository<Promotion> promotionRepository;
        private GenericRepository<Order> orderRepository;

        public OrderService(ApplicationDBContext context)
        {
            _context = context;
            batchRepository = new BatchRepository(_context);
            koiRepository = new KoiRepository(_context);
            promotionRepository = new GenericRepository<Promotion>(_context);
            orderRepository = new GenericRepository<Order>(_context);
        }

        // Go to upper comment to understand flow 
        // Create standalone CreateOrder if you like
        public Order CreateOrder(int customerID, int promotionID = 0)
        {
            // If promotionID present and not exist in db, cancel create
            if (promotionID > 0 && promotionRepository.GetById(promotionID) == null)
            {
                return null;
            }

            Order order = new Order();

            order.CreateAt = DateTime.Now;
            order.Status = OrderStatus.Pending;
            order.TotalAmount = TotalAmount;
            order.CustomerID = customerID;
            order.StaffID = 7; // This is for testing purpose
            order.PromotionID = null; //promotionID;

            order.OrderDetails = orderDetails;

            orderRepository.Create(order);
            orderRepository.Save();

            return order;
        }

        // Add order details from a list of:
        // - batchs (syntax:[ [batchID, quantity] ])
        // - kois (id[])
        public List<OrderDetail> AddOrderDetails(List<int[]> batchs, int[] kois)
        {
            foreach (var item in batchs)
            {
                Batch batchInfo = batchRepository.GetById(item[0]);

                if (batchInfo == null)
                {
                    return null;
                }

                OrderDetail detail = new OrderDetail();

                detail.Batch = batchInfo;
                detail.Quantity = item[1];
                detail.Type = OrderDetailType.Batch;
                detail.Price = batchInfo.Price * item[1];

                // Add money to total
                TotalAmount += detail.Price;

                orderDetails.Add(detail);
            }

            foreach (var koiID in kois)
            {
                Koi koiInfo = koiRepository.GetById(koiID);

                if (koiInfo == null)
                {
                    return null;
                }

                OrderDetail detail = new OrderDetail();

                detail.Koi = koiInfo;
                detail.Type = OrderDetailType.Koi;
                detail.Price = koiInfo.Price;

                // Add money to total
                TotalAmount += detail.Price;

                orderDetails.Add(detail);
            }

            return orderDetails;
        }

        public bool ApplyPromotion(int promotionID)
        {
            promotion= promotionRepository.GetById(promotionID);
            return promotion != null;
        }
    }
}
