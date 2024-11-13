using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
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
        private DeliveryRepository deliveryRepository;
        private readonly UserService userService;
        UnitOfWork unitOfWork;

        public OrderService(ApplicationDBContext context)
        {
            _context = context;
            batchRepository = new BatchRepository(_context);
            koiRepository = new KoiRepository(_context);
            consignmentKoiRepository = new ConsignmentKoiRepository(_context);
            promotionRepository = new GenericRepository<Promotion>(_context);
            orderRepository = new OrderRepository(_context);
            consignmentRepository = new ConsignmentRepository(_context);
            deliveryRepository = new DeliveryRepository(_context);
            userService = new UserService(_context);
            unitOfWork = new UnitOfWork(_context);
        }

        public Order GetByID(int id)
        {
            return orderRepository.GetOrderByID(id);
        }

        public List<Order> GetByUserID(int id)
        {
            return orderRepository.GetOrdersByUserID(id);
        }



        // Go to upper comment to understand flow 
        // Create standalone CreateOrder if you like
        public Order CreateOrder(int customerID, OrderType orderType, int promotionID, Consignment? consignment = null)
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
            order.StaffID = 7; // This is for testing purpose

            order.CustomerID = customerID;
            order.OrderDetails = orderDetails;

            if (promotionRepository.GetById(promotionID) != null)
            {
                order.PromotionID = promotionID; //promotionID;
            }

            orderRepository.Create(order);
            orderRepository.Save();

            // Add consignment to order  if consignment existed
            if (consignment != null)
            {
                consignment.OrderID = order.OrderID;
                order.Consignment = consignment;

                consignmentRepository.Create(consignment);
                consignmentRepository.Save();
            }

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

                // If batch not exist, cancel
                if (batchInfo == null)
                {
                    return null;
                }

                OrderDetail detail = new OrderDetail();

                detail.Batch = batchInfo;
                detail.Quantity = item[1];
                detail.Type = OrderDetailType.Batch;
                detail.Price = batchInfo.PricePerBatch * item[1];

                // If there not enough batch to buy, cancel
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

                // If koi not exist, cancel
                if (koiInfo == null)
                {
                    return null;
                }

                if (koiInfo.Status != KoiStatus.Available) break;

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
                if (consignmentKoiID < 1) break;
                ConsignmentKoi consignmentKoiInfo = consignmentKoiRepository.GetById(consignmentKoiID);

                // If consignmentKoi not exist, cancel
                if (consignmentKoiInfo == null)
                {
                    return null;
                }

                if (consignmentKoiInfo.Consignment.Status != ConsignmentStatus.available) break;

                OrderDetail detail = new OrderDetail();

                detail.ConsignmentKoi = consignmentKoiInfo;
                detail.Type = OrderDetailType.ConsignmentKoi;
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

            //int customerID = order.CustomerID ?? -1;
            //Customer customer = userService.GetCustomerByID(customerID).GetAwaiter().GetResult();
            //customer.LoyaltyPoints += 10;
            //userService.UpdateCustomer(customer);


            // Update order status to completed
            order.Status = OrderStatus.Completed;
            order.UpdateAt = DateTime.Now;

            // Update value:
            // - koi: update status to sold
            // - batch: remove quantity (none)
            // - consignmentKoi: update consignment status to finished
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Type == OrderDetailType.Koi)
                {
                    Koi koi = koiRepository.GetById(detail.KoiID.Value);

                    koi.Status = KoiStatus.Sold;

                    koiRepository.Update(koi);
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
            Order order = orderRepository.GetOrderByID(id);

            if (order == null)
            {
                return;
            }

            // Update order status to cancelled
            order.Status = OrderStatus.Cancelled;
            order.UpdateAt = DateTime.Now;
            order.Reason = "User cancel order";

            // Delete consignment if exist
            if (order.Consignment != null)
            {
                consignmentRepository.Remove(order.Consignment);
            }

            // Delete delivery if exist
            Delivery delivery = deliveryRepository.GetDeliveryByOrderId(order.OrderID);
            if (delivery != null)
            {
                deliveryRepository.Remove(delivery);
                deliveryRepository.Save();
            }

            // Update value:
            // - koi: update status to available
            // - batch: re-add quantity
            // - consignmentKoi: update consignment status to available
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Type == OrderDetailType.Koi)
                {
                    Koi koi = koiRepository.GetById(detail.KoiID.Value);

                    koi.Status = KoiStatus.Available;

                    koiRepository.Update(koi);
                }
                else if (detail.Type == OrderDetailType.Batch)
                {
                    Batch batch = batchRepository.GetById(detail.BatchID.Value);

                    batch.QuantityPerBatch += detail.Quantity.Value;

                    batchRepository.Update(batch);
                }
                else if (detail.Type == OrderDetailType.ConsignmentKoi)
                {
                    ConsignmentKoi consignmentKoi = consignmentKoiRepository.GetById(detail.ConsignmentKoiID.Value);

                    consignmentKoi.Consignment.Status = ConsignmentStatus.available;

                    consignmentKoiRepository.Update(consignmentKoi);
                }
            }

            orderRepository.Update(order);
            orderRepository.Save();
        }
    }
}