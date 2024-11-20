using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.data.Repositories;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Services;
using swp_be.Utils;


namespace swp_be.Controllers
{
    public class OrderRequest
    {
        public class ConsignmentOnOrder
        {
            public DateTime EndDate { get; set; }
            public int PriceListId { get; set; }
        }

        public int[] kois { get; set; } = [];
        public int[] consignmentKois { get; set; } = [];
        // Syntax: [ [batchID, quantity] ]
        public List<int[]> batchs { get; set; } = [];
        public int promotionID { get; set; } = 0;
        public string? address { get; set; }
        public OrderType paymentMethod { get; set; }
        public ConsignmentOnOrder? consignment { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        OrderService orderService;
        BatchService batchService;
        ConsignmentService consignmentService;
        TransactionService transactionService;
        DeliveryService deliveryService;

        public OrderController(ApplicationDBContext context)
        {
            _context = context;
            orderService = new OrderService(context);
            batchService = new BatchService(context);
            transactionService = new TransactionService(context);
            consignmentService = new ConsignmentService(context);
            deliveryService = new DeliveryService(context);
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize("staff, admin")]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return orderService.GetOrders();
        }

        [HttpGet]
        [Authorize("customer")]
        [Route("list")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer()
        {
            int userID = int.Parse(User.FindFirstValue("userID"));

            return orderService.GetByUserID(userID);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders/Create
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("all")]
        [Route("create")]
        public async Task<ActionResult> CreateOrder(OrderRequest orderRequest)
        {

            int userID = int.Parse(User.FindFirstValue("userID"));

            List<OrderDetail> orderDetails = orderService.AddOrderDetails(orderRequest.batchs, orderRequest.kois, orderRequest.consignmentKois);

            if (orderDetails == null || orderDetails.Count == 0)
            {
                return BadRequest(new { message = "Can't create order Details!" });
            }

            // Set every koi to be consignmentKoi if user want to consign after order
            Consignment consignment = null;

            if (orderRequest.consignment != null)
            {
                consignment = new Consignment();
                var price = await consignmentService.GetPriceListByID(orderRequest.consignment.PriceListId);

                if (price == null)
                {
                    return BadRequest(new { message = "Can't get Foster Price" });
                }

                consignment.CustomerID = userID;
                consignment.ConsignmentPriceListID = orderRequest.consignment.PriceListId;
                consignment.Type = ConsignmentType.Foster;
                consignment.Status = ConsignmentStatus.raising;
                consignment.CreateAt = DateTime.Now;
                consignment.StartDate = DateTime.Now;
                consignment.EndDate = orderRequest.consignment.EndDate;
                consignment.FosterPrice = (long)Math.Ceiling((consignment.EndDate - consignment.StartDate).TotalDays + 1) * price.PricePerDay;

                foreach (OrderDetail orderDetail in orderDetails)
                {
                    if (orderDetail.Type == OrderDetailType.Koi)
                    {
                        ConsignmentKoi consignmentKoi = new ConsignmentKoi
                        {
                            Name = orderDetail.Koi.Name,
                            Species = orderDetail.Koi.Species,
                            Price = orderDetail.Koi.Price,
                            Age = orderDetail.Koi.Age,
                            Gender = orderDetail.Koi.Gender,
                            Size = orderDetail.Koi.Size,
                            Color = orderDetail.Koi.Color,
                            DailyFeedAmount = orderDetail.Koi.DailyFeedAmount,
                            Personality = orderDetail.Koi.Personality,
                            Origin = orderDetail.Koi.Origin,
                            SelectionRate = orderDetail.Koi.SelectionRate,
                            Image = orderDetail.Koi.Image,
                            AddOn = new AddOn() {
                                HealthCertificate = orderDetail.Koi.AddOn?.HealthCertificate,
                                OriginCertificate = orderDetail.Koi.AddOn?.OriginCertificate,
                                OwnershipCertificate = orderDetail.Koi.AddOn?.OwnershipCertificate,
                            },
                        };

                        consignment.ConsignmentKois.Add(consignmentKoi);
                    }
                    else if (orderDetail.Type == OrderDetailType.ConsignmentKoi)
                    {
                        ConsignmentKoi consignmentKoi = new ConsignmentKoi
                        {
                            Name = orderDetail.ConsignmentKoi.Name,
                            Species = orderDetail.ConsignmentKoi.Species,
                            Price = orderDetail.ConsignmentKoi.Price,
                            Age = orderDetail.ConsignmentKoi.Age,
                            Gender = orderDetail.ConsignmentKoi.Gender,
                            Size = orderDetail.ConsignmentKoi.Size,
                            Color = orderDetail.ConsignmentKoi.Color,
                            DailyFeedAmount = orderDetail.ConsignmentKoi.DailyFeedAmount,
                            Personality = orderDetail.ConsignmentKoi.Personality,
                            Origin = orderDetail.ConsignmentKoi.Origin,
                            SelectionRate = orderDetail.ConsignmentKoi.SelectionRate,
                            Image = orderDetail.ConsignmentKoi.Image,
                            AddOn = new AddOn()
                            {
                                HealthCertificate = orderDetail.ConsignmentKoi.AddOn?.HealthCertificate,
                                OriginCertificate = orderDetail.ConsignmentKoi.AddOn?.OriginCertificate,
                                OwnershipCertificate = orderDetail.ConsignmentKoi.AddOn?.OwnershipCertificate,
                            },
                        };

                        consignment.ConsignmentKois.Add(consignmentKoi);
                    }
                }
            }

            var order = orderService.CreateOrder(userID, orderRequest.paymentMethod, orderRequest.promotionID, consignment);

            // order == null mean data was not complete or wrongly input 
            if (order == null)
            {
                return BadRequest(new { message = "Can't create order!" });
            }

            try
            {
                string paymentUrl;
                if (orderRequest.paymentMethod == OrderType.Offline)
                {
                    paymentUrl = transactionService.CreateVNPayTransaction(order, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());

                    Console.WriteLine("VNPAY URL: {0}", paymentUrl);

                    return Ok(new { paymentUrl });
                }
                else if (orderRequest.paymentMethod == OrderType.Online)
                {
                    // Create delivery if order is online
                    deliveryService.CreateDeliveryFromOrder(order, orderRequest.address);

                    // Create transaction
                    paymentUrl = transactionService.CreateVNPayTransaction(order, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());

                    Console.WriteLine("VNPAY URL: {0}", paymentUrl);

                    return Ok(new { paymentUrl });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
