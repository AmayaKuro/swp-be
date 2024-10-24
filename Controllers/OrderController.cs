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
        public int[] kois { get; set; } = [];
        public int[] consignmentKois { get; set; } = [];
        // Syntax: [ [batchID, quantity] ]
        public List<int[]> batchs { get; set; } = [];
        public int promotionID { get; set; } = 0;
        public OrderType paymentMethod { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        OrderService orderService;
        BatchService batchService;
        TransactionService transactionService;

        public OrderController(ApplicationDBContext context)
        {
            _context = context;
            orderService = new OrderService(context);
            batchService = new BatchService(context);
            transactionService = new TransactionService(context);
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
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
                return BadRequest("orderDetails not found!");
            }

            var order = orderService.CreateOrder(userID, orderRequest.paymentMethod, orderRequest.promotionID);

            // order == null mean data was not complete or wrongly input 
            if (order == null)
            {
                return BadRequest("order not found!");
            }

            string paymentUrl;


            try
            {

                if (orderRequest.paymentMethod == OrderType.Offline)
                {
                    long depositAmount = order.TotalAmount / 2;

                    paymentUrl = transactionService.CreateVNPayTransaction(order, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(), depositAmount);

                    Console.WriteLine("VNPAY URL: {0}", paymentUrl);

                    return Ok(new { paymentUrl });
                }
                else if (orderRequest.paymentMethod == OrderType.Online)
                {
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
            //HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var ip);
            /*string paymentUrl = transactionService.CreateVNPayTransaction(order, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());

            Console.WriteLine("VNPAY URL: {0}", paymentUrl);

            return Ok(new { paymentUrl });*/
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
