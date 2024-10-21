using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    public class DeliveryRequest()
    {
      


    }

    public class DeliveryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly DeliveryService deliveryService;


        public DeliveryController(ApplicationDBContext context)
        {
            this._context = context;
            this.deliveryService = new DeliveryService(context);
        }

        // GET: api/Koi
        
        [HttpGet]
        public async Task<ActionResult<Delivery>> GetDevliver()
        {
            return Ok(await deliveryService.GetDeliveries());
          
        }

        // GET: api/Koi/5
        [Authorize("admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(int id)
        {
            Delivery delivery = deliveryService.GetDeliveryById(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }
        [Authorize("admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliver(int id, Delivery delivery)
        {
            if (id != delivery.DeliveryID)
            {
                return BadRequest();
            }

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await deliveryService.UpdateDelivery(delivery);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliverExists(id))
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

        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDelivery(
        [FromQuery] int orderId,
        [FromQuery] int customerId,
        [FromQuery] string status,
        [FromQuery] DateTime startDeliDay,
        [FromQuery] DateTime? endDeliDay = null)
        {
            if (string.IsNullOrWhiteSpace(status) || status.Length > 50)
            {
                return BadRequest("Invalid status.");
            }

            var delivery = new Delivery
            {
                OrderID = orderId,
                CustomerID = customerId,
                Status = status,
                StartDeliDay = startDeliDay,
                EndDeliDay = endDeliDay
            };

            _context.Deliveries.Add(delivery);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateDelivery), new { id = delivery.DeliveryID }, delivery);
            }
            catch (Exception ex)
            {
                // Handle exceptions such as database issues
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Koi/5
        [Authorize("admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKoi(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            await deliveryService.DeleteDelivery(delivery);

            return NoContent();
        }

        private bool DeliverExists(int id)
        {
            return _context.Deliveries.Any(d => d.DeliveryID == id);
        }
    }
}