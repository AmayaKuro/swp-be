using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using System.Drawing;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentKoiController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ConsignmentKoiService ConsignmentKoiservice;

        public ConsignmentKoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.ConsignmentKoiservice = new ConsignmentKoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> GetFosterKoi()
        {
            return Ok(await ConsignmentKoiservice.GetConsignmentKois());
        }

        // GET: api/Koi/5        
        [HttpGet("{id}")]
        [Authorize("all")]
        public async Task<ActionResult<ConsignmentKoi>> GetFosterKoi(int id)
        {
            var consignmentKoi = await _context.ConsignmentKois.FindAsync(id);

            if (consignmentKoi == null)
            {
                return NotFound();
            }

            return consignmentKoi;
        }
        [HttpGet("search")]
        [Authorize("all")]
        public async Task<ActionResult<IEnumerable<ConsignmentKoi>>> Search(
            string? name = null,
            string? color = null,
            string? size = null,
            string? gender = null
            )

        {
            var query = _context.ConsignmentKois.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(koi => koi.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(koi => koi.Color.Contains(color));
            }

            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(koi => koi.Size.Contains(size));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(koi => koi.Gender.Contains(gender));
            }

            var results = await query.ToListAsync();
            return Ok(results);
        }


        // PUT: api/Koi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateFosterKoi(
        [FromQuery] int id,
        [FromQuery] string? name,
        [FromQuery] string? gender,
        [FromQuery] int? age,
        [FromQuery] string? size,
        [FromQuery] string? color,
        [FromQuery] string? dailyFeedAmount,
        [FromQuery] string? personality,
        [FromQuery] string? origin,
        [FromQuery] string? selectionRate,
        [FromQuery] string? species,
        [FromQuery] long? price,
        [FromQuery] int? fosteringDays,
        [FromQuery] int? consignmentId)
        {
            // Find the foster koi, ensuring you await the result
            var consignmentKoi = await _context.ConsignmentKois.FindAsync(id);

            // Check if foster koi exists
            if (consignmentKoi == null)
            {
                return NotFound();
            }

            // Validate species if provided
            if (!string.IsNullOrWhiteSpace(species) && species.Length > 255)
            {
                return BadRequest("Invalid species.");
            }

            // Validate fostering days if provided
            if (fosteringDays.HasValue && fosteringDays <= 0)
            {
                return BadRequest("Fostering days must be greater than zero.");
            }

            // Update foster koi properties only if they are provided
            if (name != null) consignmentKoi.Name = name;
            if (gender != null) consignmentKoi.Gender = gender;
            if (age.HasValue) consignmentKoi.Age = age.Value;
            if (size != null) consignmentKoi.Size = size;
            if (color != null) consignmentKoi.Color = color;
            if (dailyFeedAmount != null) consignmentKoi.DailyFeedAmount = dailyFeedAmount;
            if (personality != null) consignmentKoi.Personality = personality;
            if (origin != null) consignmentKoi.Origin = origin;
            if (selectionRate != null) consignmentKoi.SelectionRate = selectionRate;
            if (species != null) consignmentKoi.Species = species;
            if (price.HasValue) consignmentKoi.Price = price.Value;
            if (fosteringDays.HasValue) consignmentKoi.FosteringDays = fosteringDays.Value;
            if (consignmentId.HasValue) consignmentKoi.ConsignmentID = consignmentId.Value;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(consignmentKoi);
        }



        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
        [HttpPost("CreateFosterKoi")]
        public async Task<IActionResult> CreateFosterKoi(
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
          [FromQuery] long pricePerDay,
          [FromQuery] int fosteringDays,
          [FromQuery] int consignmentId)
        {
            if (string.IsNullOrWhiteSpace(species) || species.Length > 255)
            {
                return BadRequest("Invalid species.");
            }

            if (fosteringDays <= 0)
            {
                return BadRequest("Fostering days must be greater than zero.");
            }

            var consignmentKoi = new ConsignmentKoi
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
                Price = pricePerDay,
                FosteringDays = fosteringDays,
                ConsignmentID = consignmentId
            };

            _context.ConsignmentKois.Add(consignmentKoi);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateFosterKoi), new { id = consignmentKoi.ConsignmentKoiID }, consignmentKoi);
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
            var koi = await _context.ConsignmentKois.FindAsync(id);
            if (koi == null)
            {
                return NotFound();
            }

            await ConsignmentKoiservice.DeleteKoi(koi);

            return NoContent();
        }

        private bool KoiExists(int id)
        {
            return _context.ConsignmentKois.Any(e => e.ConsignmentKoiID == id);
        }
    }
}
