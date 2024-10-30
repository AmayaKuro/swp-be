using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Services;
using swp_be.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace swp_be.Controllers
{
    public class ConsignmentRequest
    {
        public int ConsignmentID { get; set; }
        public int CustomerID { get; set; }
        public ConsignmentType Type { get; set; }
        public long FosterPrice { get; set; }
        public ConsignmentStatus Status { get; set; }
    }
    public class ConsignKoiRequest
    {
        public int CustomerID { get; set; }
        public ConsignmentType Type { get; set; }
        public ConsignmentStatus Status { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? DailyFeedAmount { get; set; }
        public string? Personality { get; set; }
        public string? Origin { get; set; }
        public string? SelectionRate { get; set; }
        public string Species { get; set; }
        public long PricePerDay { get; set; }
        public int FosteringDays { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ConsignmentService consignmentService;
        private readonly TransactionService transactionService;
        private readonly ConsignmentKoiService consignmentKoiService; 
        public ConsignmentController(ApplicationDBContext context)
        {
            this._context = context;
            this.consignmentService= new ConsignmentService(context);
            transactionService = new TransactionService(context);
            consignmentKoiService=new ConsignmentKoiService(context);
        }
        [HttpGet]
        public async Task<ActionResult<Consignment>> GetConsignment()
        {
            var consignments = await consignmentService.GetConsignment();
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
        [Authorize("staff, admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsignment(ConsignmentRequest consignmentRequest)
        {
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
            //if (consignment.Status != ConsignmentStatus.pending)
            //{
            //    string paymentUrl = transactionService.CreateVNPayTransaction(consignment, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            //}
            // Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Error updating consignment" });
            }

            return Ok(new { message = "Consignment updated successfully" });
        }
        [Authorize("staff, admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateConsignment(ConsignKoiRequest consignKoiRequest )
            {
                // Create a new consignment object
                var newConsignment = new Consignment
                {
                CustomerID = consignKoiRequest.CustomerID,
                Type = consignKoiRequest.Type,
                FosterPrice = consignKoiRequest.FosteringDays * consignKoiRequest.PricePerDay,
                Status = consignKoiRequest.Status // Ensure this is correctly spelled
                };
            // Create a new ConsignmentKoi object
            var fosterKoi = new ConsignmentKoi
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
                Price = consignKoiRequest.PricePerDay,
                FosteringDays = consignKoiRequest.FosteringDays,
                ConsignmentID = newConsignment.ConsignmentID // Set this only after saving the consignment
            };

            // Add the new consignment to the database
            _context.Consignments.Add(newConsignment);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();

                // Set the ConsignmentID after saving to the database
                fosterKoi.ConsignmentID = newConsignment.ConsignmentID;

                // Add the Koi after the consignment is saved
                _context.ConsignmentKois.Add(fosterKoi);
                await _context.SaveChangesAsync(); // Save the Koi as well
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for debugging purposes if needed
                return StatusCode(500, new { message = "Error creating consignment", details = ex.Message });
            }
            string paymentUrl = transactionService.CreateVNPayTransaction(newConsignment,HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            return Ok(new { paymentUrl });
        }
       
        [Route("pending")]
        [HttpPost]
        public async Task<IActionResult> NegotiatingConsignment(
        [FromQuery] ConsignmentType type,
        [FromQuery] string? name,
        [FromQuery] string? gender,
        [FromQuery] int? age,
        [FromQuery] string? size,
        [FromQuery] string? color,
        [FromQuery] string? dailyFeedAmount,
        [FromQuery] string? personality,
        [FromQuery] string? origin,
        [FromQuery] string? selectionRate,
        [FromQuery] string species,
        [FromQuery] int fosteringDays)
        {
            // Retrieve the customer ID from the user's claims
            int customerID = int.Parse(User.FindFirstValue("userID"));

            // Create a new consignment object
            var newConsignment = new Consignment
            {
                CustomerID = customerID,
                Type = type,
                Status = ConsignmentStatus.negotiate // Ensure this is correctly spelled
            };

            // Create a new ConsignmentKoi object
            var fosterKoi = new ConsignmentKoi
            {
                Name = name,
                Gender = gender,
                Age = age,
                Size = size,
                Color = color,
                DailyFeedAmount = dailyFeedAmount,
                Personality = personality,
                Origin = origin,
                SelectionRate = selectionRate,
                Species = species,
                //Price=null
                FosteringDays = fosteringDays,
                ConsignmentID = newConsignment.ConsignmentID // Set this only after saving the consignment
            };

            // Add the new consignment to the database
            _context.Consignments.Add(newConsignment);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();

                // Set the ConsignmentID after saving to the database
                fosterKoi.ConsignmentID = newConsignment.ConsignmentID;

                // Add the Koi after the consignment is saved
                _context.ConsignmentKois.Add(fosterKoi);
                await _context.SaveChangesAsync(); // Save the Koi as well
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for debugging purposes if needed
                return StatusCode(500, new { message = "Error creating consignment", details = ex.Message });
            }
            return Ok(new { message = "Consignment created successfully", consignmentID = newConsignment.ConsignmentID });

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
            var results = await consignmentService.SearchConsignments(customerID,type,status,minFosterPrice,maxFosterPrice);

            // Return the result
            return Ok(results);
        }
    }
}
