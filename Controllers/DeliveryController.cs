﻿using System;
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
using swp_be.Utils;

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
        public string? Address { get; set; }
        public string? Reason { get; set; }
        public IFormFile? ReasonImage { get; set; }
    }
    [Route("api/koi/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly DeliveryService deliveryService;
        private readonly FirebaseUtils fbUtils = new FirebaseUtils();

        public DeliveryController(ApplicationDBContext context)
        {
            this._context = context;
            this.deliveryService = new DeliveryService(context);
        }

        // GET: api/Koi
        
        [HttpGet]
        [Authorize("all")]
        public async Task<ActionResult<Delivery>> GetDevliver()
        {
            return Ok(await deliveryService.GetDeliveries());
          
        }
        // GET: api/Koi/5
        [Authorize("all")]
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
        [Authorize("all")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliver(DeliveryRequest deliveryRequest,int id)
        {
            deliveryRequest.DeliveryID=id;
            Delivery delivery = deliveryService.GetDeliveryById(deliveryRequest.DeliveryID);
            if (delivery == null)
            {
                return BadRequest();
            }
            delivery.StartDeliDay = deliveryRequest.StartDeliDay ?? delivery.StartDeliDay;
            delivery.EndDeliDay = deliveryRequest.EndDeliDay ?? delivery.EndDeliDay;

            if (delivery.StartDeliDay > delivery.EndDeliDay)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            delivery.OrderID = deliveryRequest.OrderID ?? delivery.OrderID;
            delivery.CustomerID = deliveryRequest.CustomerID ?? delivery.CustomerID;
            delivery.Status = deliveryRequest.Status ?? delivery.Status;
            delivery.Address = deliveryRequest.Address ?? delivery.Address;
            delivery.Reason= deliveryRequest.Reason ?? delivery.Reason;
            delivery.ReasonImage= await fbUtils.UploadImage(deliveryRequest.ReasonImage?.OpenReadStream(),deliveryRequest.DeliveryID.ToString(), "ReasonImage");
            await deliveryService.UpdateDelivery(delivery);
            return NoContent();
        }

        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("all")]
        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromForm]DeliveryRequest deliveryRequest)
        {
            if (deliveryRequest.StartDeliDay > deliveryRequest.EndDeliDay)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            if (deliveryRequest.StartDeliDay == null)
            {
                return BadRequest("StartDeliDay is required.");
            }

            if (deliveryRequest.OrderID == null)
            {
                return BadRequest("OrderID is required.");
            }

            if (deliveryRequest.CustomerID == null)
            {
                return BadRequest("CustomerID is required.");
            }

            if (deliveryRequest.Status == null)
            {
                return BadRequest("Status is required.");
            }

            Delivery delivery = new Delivery();
            await fbUtils.UploadImage(deliveryRequest.ReasonImage?.OpenReadStream(), deliveryRequest.DeliveryID.ToString(), "ReasonImage");
            delivery.StartDeliDay = deliveryRequest.StartDeliDay.Value;
            delivery.EndDeliDay = deliveryRequest.EndDeliDay;
            delivery.OrderID = deliveryRequest.OrderID.Value;
            delivery.CustomerID = deliveryRequest.CustomerID.Value;
            delivery.Status = deliveryRequest.Status.Value;
            delivery.Address = deliveryRequest.Address;
            delivery.Reason=deliveryRequest.Reason;
            delivery.ReasonImage = await fbUtils.UploadImage(deliveryRequest.ReasonImage?.OpenReadStream(), deliveryRequest.DeliveryID.ToString(), "ReasonImage"); ;
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