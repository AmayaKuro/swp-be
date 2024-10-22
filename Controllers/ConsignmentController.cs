using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Services;
using swp_be.Models;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ConsignmentService consignmentService;
        public ConsignmentController(ApplicationDBContext context)
        {
            this._context = context;
            this.consignmentService= new ConsignmentService(context);
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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateConsignment(
              [FromQuery] int consignmentID,
              [FromQuery] int customerID,
              [FromQuery] ConsignmentType type,
              [FromQuery] decimal fosterPrice,
              [FromQuery] ConsigmentStatus status)
        {
            // Find the consignment by ID
            var consignment = await _context.Consignments.FindAsync(consignmentID);

            if (consignment == null)
            {
                return NotFound(new { message = "Consignment not found" });
            }

            // Update the consignment properties
            consignment.CustomerID = customerID;
            consignment.Type = type;
            consignment.FosterPrice = fosterPrice;
            consignment.Status = status;

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
        [HttpPost("create")]
        public async Task<IActionResult> CreateConsignment(
           [FromQuery] int customerID,
           [FromQuery] ConsignmentType type,
           [FromQuery] decimal fosterPrice,
           [FromQuery] ConsigmentStatus status)
        {
            // Create a new consignment object
            var newConsignment = new Consignment
            {
                CustomerID = customerID,
                Type = type,
                FosterPrice = fosterPrice,
                Status = status
            };

            // Add the new consignment to the database
            _context.Consignments.Add(newConsignment);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { message = "Error creating consignment" });
            }

            return Ok(new { message = "Consignment created successfully", consignmentID = newConsignment.ConsignmentID });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsignment(int id)
        {
            var consignment = await _context.Consignments.FindAsync(id);
            if (consignment == null)
            {
                return NotFound();
            }

            await consignmentService.DeleteConsignment(consignment);

            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchConsignments(
           [FromQuery] int? customerID = null,
           [FromQuery] ConsignmentType? type = null,
           [FromQuery] ConsigmentStatus? status = null,
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
