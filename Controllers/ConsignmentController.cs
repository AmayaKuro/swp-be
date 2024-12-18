﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Services;
using swp_be.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using swp_be.Utils;
using swp_be.data.Repositories;

namespace swp_be.Controllers
{
    public class ConsignmentRequest
    {
        public int ConsignmentID { get; set; }
        public int CustomerID { get; set; }
        public ConsignmentType Type { get; set; }
        public long FosterPrice { get; set; }
        public ConsignmentStatus Status { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int PriceListId { get; set; }
    }
    public class ConsignKoiRequest
    {
        public int ConsignKoiID { get; set; }
        public int ConsignmentID { get; set; }
        public int CustomerID { get; set; }
        public ConsignmentType Type { get; set; }
        public ConsignmentStatus Status { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int PriceListId { get; set; }
        //Bien cua consignKoi
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? DailyFeedAmount { get; set; }
        public string? Personality { get; set; }
        public string? Origin { get; set; }
        public string? SelectionRate { get; set; }
        public string? Species { get; set; }
        public long Price { get; set; }

        public IFormFile? Image { get; set; }
        public IFormFile? OriginCertificate { get; set; }
        public IFormFile? HealthCertificate { get; set; }
        public IFormFile? OwnershipCertificate { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ConsignmentService consignmentService;
        private readonly GenericRepository<ConsignmentPriceList> consignmentPriceListRepository;
        private readonly TransactionService transactionService;
        private readonly ConsignmentKoiService consignmentKoiService;
        private readonly FirebaseUtils fbUtils = new FirebaseUtils();

        public ConsignmentController(ApplicationDBContext context)
        {
            this._context = context;
            consignmentService = new ConsignmentService(context);
            transactionService = new TransactionService(context);
            consignmentKoiService = new ConsignmentKoiService(context);
            consignmentPriceListRepository = new GenericRepository<ConsignmentPriceList>(context);
        }

        [HttpGet]
        public async Task<ActionResult<Consignment>> GetConsignment()
        {
            var consignments = await consignmentService.GetConsignment();
            return Ok(consignments);
        }

        [HttpGet]
        [Authorize("all")]
        [Route("CustomerConsignment")]
        public async Task<ActionResult<Consignment>> GetConsignmentByUser()
        {
            int customerID = int.Parse(User.FindFirstValue("userID"));
            var consignments = await consignmentService.GetConsignmentByCustomer(customerID);
            return Ok(consignments);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Consignment>> GetConsignment(int id)
        {
            var consignment = await consignmentService.GetById(id);

            if (consignment == null)
            {
                return NotFound();
            }

            return consignment;
        }

        [Route("priceList")]
        [HttpGet]
        public async Task<ActionResult<Consignment>> GetPriceList()
        {
            var priceList = await consignmentService.GetPriceList();

            return Ok(priceList);
        }

        [Authorize("staff, admin")]
        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateConsignment(ConsignmentRequest consignmentRequest)
        {
            if (consignmentRequest.FosterPrice < 0 || consignmentRequest.StartDate > consignmentRequest.EndDate)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            // Find the consignment by ID
            var consignment = await _context.Consignments.FindAsync(consignmentRequest.ConsignmentID);

            if (consignment == null)
            {
                return NotFound(new { message = "Consignment not found" });
            }

            // Update the consignment properties
            consignment.CustomerID = consignmentRequest.CustomerID;
            consignment.Type = consignmentRequest.Type;
            consignment.FosterPrice = consignmentRequest.FosterPrice;
            consignment.Status = consignmentRequest.Status;
            consignment.StartDate = consignmentRequest.StartDate;
            consignment.EndDate = consignmentRequest.EndDate;
            consignment.ConsignmentPriceListID = consignmentRequest.PriceListId;
            //if (consignment.Status != ConsignmentStatus.pending)
            //{
            //    string paymentUrl = transactionService.CreateVNPayTransaction(consignment, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            //}
            // Save changes
            try
            {
                await consignmentService.UpdateConsignment(consignment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating consignment" });
            }

            return Ok(new { message = "Consignment updated successfully" });
        }

        [Authorize("staff, admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateConsignment([FromForm] ConsignKoiRequest consignKoiRequest)
        {
            if (consignKoiRequest.Price < 0 || consignKoiRequest.StartDate >  consignKoiRequest.EndDate)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            var priceList = consignmentPriceListRepository.GetById(consignKoiRequest.PriceListId);

            if (priceList == null)
            {
                return BadRequest(new { message = "Price type not found" });
            }

            // Create a new consignment object
            var newConsignment = new Consignment
            {
                CustomerID = consignKoiRequest.CustomerID,
                Type = consignKoiRequest.Type,
                FosterPrice = (long)Math.Ceiling((consignKoiRequest.EndDate - consignKoiRequest.StartDate).TotalDays + 1) * priceList.PricePerDay,
                Status = consignKoiRequest.Status, // Ensure this is correctly spelled
                CreateAt = DateTime.Now,
                StartDate = consignKoiRequest.StartDate,
                EndDate = consignKoiRequest.EndDate,
                ConsignmentPriceListID = consignKoiRequest.PriceListId
            };
            // Create a new ConsignmentKoi object
            var consignmentKoi = new ConsignmentKoi
            {
                Name = consignKoiRequest.Name,
                Gender = consignKoiRequest.Gender,
                Age = consignKoiRequest.Age,
                Size = consignKoiRequest.Size,
                Color = consignKoiRequest.Color,
                DailyFeedAmount = consignKoiRequest.Color,
                Personality = consignKoiRequest.Color,
                Origin = consignKoiRequest.Color,
                SelectionRate = consignKoiRequest.Color,
                Species = consignKoiRequest.Species,
                Price = consignKoiRequest.Price,
                ConsignmentID = newConsignment.ConsignmentID,
                AddOn = new AddOn()
                // Set this only after saving the consignment
            };

            // Add the new consignment to the database
            await consignmentService.CreateConsignment(newConsignment, consignmentKoi);
            consignmentKoi.Image = await fbUtils.UploadImage(consignKoiRequest.Image?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "koiImage");
            consignmentKoi.AddOn.OriginCertificate = await fbUtils.UploadImage(consignKoiRequest.OriginCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "originCertificate");
            consignmentKoi.AddOn.HealthCertificate = await fbUtils.UploadImage(consignKoiRequest.HealthCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "healthCertificate");
            consignmentKoi.AddOn.OwnershipCertificate = await fbUtils.UploadImage(consignKoiRequest.OwnershipCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "ownershipCertificate");
            await _context.SaveChangesAsync();
            string paymentUrl = transactionService.CreateVNPayTransaction(newConsignment, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            return Ok(new { paymentUrl });
        }

        [Route("pending")]
        [HttpPost]
        public async Task<IActionResult> NegotiatingConsignment([FromForm] ConsignKoiRequest consignKoiRequest)
        {
            if (consignKoiRequest.Price < 0 || consignKoiRequest.StartDate > consignKoiRequest.EndDate)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            // Retrieve the customer ID from the user's claims
            int customerID = int.Parse(User.FindFirstValue("userID"));

            var priceList = consignmentPriceListRepository.GetById(consignKoiRequest.PriceListId);

            if (priceList == null)
            {
                return BadRequest(new { message = "Price type not found" });
            }

            // Create a new consignment object

            var newConsignment = new Consignment();
            if (consignKoiRequest.Type == (ConsignmentType.Sell))
            {


                newConsignment.CustomerID = customerID;
                newConsignment.Type = consignKoiRequest.Type;
                newConsignment.CreateAt = DateTime.Now;
                newConsignment.StartDate = consignKoiRequest.StartDate;
                newConsignment.EndDate = consignKoiRequest.EndDate;
                newConsignment.FosterPrice = (long)Math.Ceiling((consignKoiRequest.EndDate - consignKoiRequest.StartDate).TotalDays + 1) * priceList.PricePerDay;
                newConsignment.Status = ConsignmentStatus.pending; // Ensure this is correctly spelled
                newConsignment.ConsignmentPriceListID = consignKoiRequest.PriceListId;

            }
            else if (consignKoiRequest.Type == (ConsignmentType.Foster))
            {


                newConsignment.CustomerID = customerID;
                newConsignment.Type = consignKoiRequest.Type;
                newConsignment.CreateAt = DateTime.Now;
                newConsignment.StartDate = consignKoiRequest.StartDate;
                newConsignment.EndDate = consignKoiRequest.EndDate;
                newConsignment.FosterPrice = (long)Math.Ceiling((consignKoiRequest.EndDate - consignKoiRequest.StartDate).TotalDays + 1) * priceList.PricePerDay;
                newConsignment.Status = ConsignmentStatus.pending; // Ensure this is correctly spelled
                newConsignment.ConsignmentPriceListID = consignKoiRequest.PriceListId;
            }
            // Create a new ConsignmentKoi object
            var consignmentKoi = new ConsignmentKoi
            {
                Name = consignKoiRequest.Name,
                Gender = consignKoiRequest.Gender,
                Age = consignKoiRequest.Age,
                Size = consignKoiRequest.Size,
                Color = consignKoiRequest.Color,
                DailyFeedAmount = consignKoiRequest.DailyFeedAmount,
                Personality = consignKoiRequest.Personality,
                Origin = consignKoiRequest.Origin,
                SelectionRate = consignKoiRequest.SelectionRate,
                Species = consignKoiRequest.Species,
                Price = consignKoiRequest.Price,
                ConsignmentID = newConsignment.ConsignmentID, // Set this only after saving the consignment
                AddOn = new AddOn()
            };

            // Add the new consignment to the database
            _context.Consignments.Add(newConsignment);
            consignmentKoi.Image = await fbUtils.UploadImage(consignKoiRequest.Image?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "koiImage");
            consignmentKoi.AddOn.OriginCertificate = await fbUtils.UploadImage(consignKoiRequest.OriginCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "originCertificate");
            consignmentKoi.AddOn.HealthCertificate = await fbUtils.UploadImage(consignKoiRequest.HealthCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "healthCertificate");
            consignmentKoi.AddOn.OwnershipCertificate = await fbUtils.UploadImage(consignKoiRequest.OwnershipCertificate?.OpenReadStream(), consignmentKoi.ConsignmentKoiID.ToString(), "ownershipCertificate");
            await _context.SaveChangesAsync();
            // Save changes to the database

            try
            {
                await _context.SaveChangesAsync();
                // Set the ConsignmentID after saving to the database
                consignmentKoi.ConsignmentID = newConsignment.ConsignmentID;

                // Add the Koi after the consignment is saved
                _context.ConsignmentKois.Add(consignmentKoi);
                await _context.SaveChangesAsync(); // Save the Koi as well
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for debugging purposes if needed
                return StatusCode(500, new { message = "Error creating consignment", details = ex.Message });
            }
            string paymentUrl = transactionService.CreateVNPayTransaction(newConsignment, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            return Ok(new
            {
                message = "Consignment created successfully",
                consignmentID = newConsignment.ConsignmentID,
                consignmentKoiID = consignmentKoi.ConsignmentKoiID,
                paymentUrl
            });
        }

        [Authorize("staff, admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsignment(int id)
        {
            var consignment = await consignmentService.GetById(id);
            if (consignment == null)
            {
                return NotFound();
            }

            var success = await consignmentService.DeleteConsignment(consignment);
            if (success)
            {
                return Ok(consignment);
            }
            return BadRequest("Failed to delete the consignment.");
        }

        [HttpGet("search")]
        [Authorize("all")]
        public async Task<IActionResult> SearchConsignments(
           [FromQuery] int? customerID = null,
           [FromQuery] ConsignmentType? type = null,
           [FromQuery] ConsignmentStatus? status = null,
           [FromQuery] decimal? minFosterPrice = null,
           [FromQuery] decimal? maxFosterPrice = null)
        {
            // Call the SearchConsignments method
            var results = await consignmentService.SearchConsignments(customerID, type, status, minFosterPrice, maxFosterPrice);

            // Return the result
            return Ok(results);
        }

        [Authorize("staff, admin")]
        [Route("Resign")]
        [HttpPut]
        public async Task<IActionResult> Reasign(ConsignmentRequest consignmentRequest)
        {
            if (consignmentRequest.StartDate > consignmentRequest.EndDate)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            // Find the consignment by ID
            int customerID = int.Parse(User.FindFirstValue("userID"));
            var consignment = await _context.Consignments.FindAsync(consignmentRequest.ConsignmentID);
            if (consignment == null)
            {
                return NotFound(new { message = "Consignment not found" });
            }
            var priceList = consignmentPriceListRepository.GetById(consignmentRequest.PriceListId);

            if (priceList == null)
            {
                return BadRequest(new { message = "Price type not found" });
            }

            // Update the consignment properties
            consignment.CustomerID = customerID;
            consignment.Type = consignmentRequest.Type;
            consignment.FosterPrice = (long)Math.Ceiling((consignmentRequest.EndDate - consignmentRequest.StartDate).TotalDays + 1) * priceList.PricePerDay;
            consignment.Status = consignmentRequest.Status;
            consignment.StartDate = consignmentRequest.StartDate;
            consignment.EndDate = consignmentRequest.EndDate;
            consignment.ConsignmentPriceListID = consignmentRequest.PriceListId;
            //if (consignment.Status != ConsignmentStatus.pending)
            //{
            //    string paymentUrl = transactionService.CreateVNPayTransaction(consignment, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            //}
            // Save changes
            try
            {
                await consignmentService.UpdateConsignment(consignment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating consignment" });
            }

            return Ok(new { message = "Consignment updated successfully" });
        }
    }
}
