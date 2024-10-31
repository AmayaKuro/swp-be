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
        public long TotalAmount { get; private set; }
        public Promotion promotion { get; set; }
        public List<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();


        private readonly ApplicationDBContext _context;
        private BatchRepository batchRepository;
        private KoiRepository koiRepository;
        private ConsignmentKoiRepository consignmentKoiRepository;
        private GenericRepository<Promotion> promotionRepository;
        private OrderRepository orderRepository;
        private ConsignmentRepository consignmentRepository;

        public OrderService(ApplicationDBContext context)
        {
            _context = context;
            batchRepository = new BatchRepository(_context);
            koiRepository = new KoiRepository(_context);
            consignmentKoiRepository = new ConsignmentKoiRepository(_context);
            promotionRepository = new GenericRepository<Promotion>(_context);
            orderRepository = new OrderRepository(_context);
            consignmentRepository = new ConsignmentRepository(_context);
        }

        public Order GetByID(int id)
        {
            return orderRepository.GetById(id);
        }

        public List<Order> GetByUserID(int id)
        {
            return orderRepository.GetOrdersByUserID(id);
        }



        // Go to upper comment to understand flow 
        // Create standalone CreateOrder if you like
        public Order CreateOrder(int customerID, OrderType orderType, int promotionID)
        {
            // If promotionID present and not exist in db, cancel create
            if (promotionID > 0 && promotionRepository.GetById(promotionID) == null)
            {
                return null;
            }

            Order order = new Order();

            order.CreateAt = DateTime.Now;
            order.Status = OrderStatus.Pending;
            order.Type = orderType;
            order.TotalAmount = TotalAmount;
            order.CustomerID = customerID;
            order.StaffID = 7; // This is for testing purpose
            if (promotionRepository.GetById(promotionID) != null)
            {
                order.PromotionID = promotionID; //promotionID;
            }
            

            order.OrderDetails = orderDetails;

            orderRepository.Create(order);
            orderRepository.Save();

            return order;
        }

        // Add order details from a list of:
        // - batchs (syntax:[ [batchID, quantity] ])
        // - kois (id[])
        // - consignmentKois (id[])
        public List<OrderDetail> AddOrderDetails(List<int[]> batchs, int[] kois, int[] consignmentKois)
        {
            foreach (var item in batchs)
            {
                if (item[0] < 1) break;

                Batch batchInfo = batchRepository.GetById(item[0]);

                if (batchInfo == null)
                {
                    return null;
                }

                OrderDetail detail = new OrderDetail();

                detail.Batch = batchInfo;
                detail.Quantity = item[1];
                detail.Type = OrderDetailType.Batch;
                detail.Price = batchInfo.PricePerBatch * item[1];

                if (batchInfo.RemainBatch < item[1])
                {
                    return null; 
                }
                batchInfo.RemainBatch -= item[1];
                batchRepository.Update(batchInfo);

                // Add money to total
                TotalAmount += detail.Price;

                orderDetails.Add(detail);
            }

            foreach (var koiID in kois)
            {
                if (koiID < 1) break;

                Koi koiInfo = koiRepository.GetById(koiID);

                if (koiInfo.Status != KoiStatus.Available) break;

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

                koiInfo.Status = KoiStatus.InOrder;
                koiRepository.Update(koiInfo);

                orderDetails.Add(detail);
            }

            foreach (var consignmentKoiID in consignmentKois)
            {
                Console.WriteLine("Conssingggment " + consignmentKoiID);
                if (consignmentKoiID < 1) break;
                ConsignmentKoi consignmentKoiInfo = consignmentKoiRepository.GetById(consignmentKoiID);

                if (consignmentKoiInfo.Consignment.Status != ConsignmentStatus.available) break;

                if (consignmentKoiInfo == null)
                {
                    return null;
                }

                OrderDetail detail = new OrderDetail();

                detail.ConsignmentKoi = consignmentKoiInfo;
                detail.Type = OrderDetailType.Koi;
                detail.Price = consignmentKoiInfo.Price;
                
                consignmentKoiInfo.Consignment.Status = ConsignmentStatus.pending;
                consignmentRepository.Update(consignmentKoiInfo.Consignment);               

                // Add money to total
                TotalAmount += detail.Price;

                orderDetails.Add(detail);
            }

            return orderDetails;
        }

        public bool ApplyPromotion(int promotionID)
        {
            promotion = promotionRepository.GetById(promotionID);
            return promotion != null;
        }

        public void FinishOrder(int id)
        {
            Order order = orderRepository.GetOrderByID(id);

            if (order == null)
            {
                return;
            }

            order.Status = OrderStatus.Completed;
            order.UpdateAt = DateTime.Now;



            foreach (var detail in order.OrderDetails)
            {
                if (detail.Type == OrderDetailType.Batch)
                {
                    Batch batch = batchRepository.GetById(detail.BatchID.Value);

                    batch.QuantityPerBatch -= detail.Quantity.Value;

                    batchRepository.Update(batch);
                }
                else if (detail.Type == OrderDetailType.ConsignmentKoi)
                {
                    ConsignmentKoi consignmentKoi = consignmentKoiRepository.GetById(detail.ConsignmentKoiID.Value);

                    consignmentKoi.Consignment.Status = ConsignmentStatus.finished;

                    consignmentKoiRepository.Update(consignmentKoi);
                }
            }

            orderRepository.Update(order);
            orderRepository.Save();
        }

        public void CancelOrder(int id)
        {
            Order order = orderRepository.GetById(id);

            if (order == null)
            {
                return;
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdateAt = DateTime.Now;

            orderRepository.Update(order);
            orderRepository.Save();
        }
    }
}