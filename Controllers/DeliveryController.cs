using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
    public class DeliveryRequest
    {
        public int DeliveryID { get; set; } = 0;
        public int? OrderID { get; set; }
        public int? CustomerID { get; set; }
        public DeliveryStatus? Status { get; set; }
        public DateTime? StartDeliDay { get; set; }
        public DateTime? EndDeliDay { get; set; }
    }

    [Route("api/koi/[controller]")]
    [ApiController]
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
        [Authorize("staff, admin")]
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
        [Authorize("staff, admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliver(DeliveryRequest deliveryRequest)
        {
            Delivery delivery = deliveryService.GetDeliveryById(deliveryRequest.DeliveryID);
            if (delivery == null)
            {
                return BadRequest();
            }

            delivery.StartDeliDay = deliveryRequest.StartDeliDay ?? delivery.StartDeliDay;
            delivery.EndDeliDay = deliveryRequest.EndDeliDay ?? delivery.EndDeliDay;
            delivery.OrderID = deliveryRequest.OrderID ?? delivery.OrderID;
            delivery.CustomerID = deliveryRequest.CustomerID ?? delivery.CustomerID;
            delivery.Status = deliveryRequest.Status ?? delivery.Status;

            await deliveryService.UpdateDelivery(delivery);
            return NoContent();
        }

        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDelivery(DeliveryRequest deliveryRequest)
        {
            Delivery delivery = new Delivery();
            

            delivery.StartDeliDay = deliveryRequest.StartDeliDay ?? delivery.StartDeliDay;
            delivery.EndDeliDay = deliveryRequest.EndDeliDay ?? delivery.EndDeliDay;
            delivery.OrderID = deliveryRequest.OrderID ?? delivery.OrderID;
            delivery.CustomerID = deliveryRequest.CustomerID ?? delivery.CustomerID;
            delivery.Status = deliveryRequest.Status ?? delivery.Status;

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
        [Authorize("staff, admin")]
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