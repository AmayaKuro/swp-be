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
        private readonly ConsignmentKoiService fosterKoiService;

        public ConsignmentKoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.fosterKoiService = new ConsignmentKoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> GetFosterKoi()
        {
            return Ok(await fosterKoiService.GetFosterKois());
        }

        // GET: api/Koi/5
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsignmentKoi>> GetFosterKoi(int id)
        {
            var fosterKoi = await _context.FosterKois.FindAsync(id);

            if (fosterKoi == null)
            {
                return NotFound();
            }

            return fosterKoi;
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ConsignmentKoi>>> Search(
            string? name = null,
            string? color = null,
            string? size = null,
            string? gender = null
            )

        {
            var query = _context.FosterKois.AsQueryable();

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
    [FromQuery] decimal? price,
    [FromQuery] int? fosteringDays,
    [FromQuery] int? consignmentId)
        {
            // Find the foster koi, ensuring you await the result
            var fosterKoi = await _context.FosterKois.FindAsync(id);

            // Check if foster koi exists
            if (fosterKoi == null)
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
            if (name != null) fosterKoi.Name = name;
            if (gender != null) fosterKoi.Gender = gender;
            if (age.HasValue) fosterKoi.Age = age.Value;
            if (size != null) fosterKoi.Size = size;
            if (color != null) fosterKoi.Color = color;
            if (dailyFeedAmount != null) fosterKoi.DailyFeedAmount = dailyFeedAmount;
            if (personality != null) fosterKoi.Personality = personality;
            if (origin != null) fosterKoi.Origin = origin;
            if (selectionRate != null) fosterKoi.SelectionRate = selectionRate;
            if (species != null) fosterKoi.Species = species;
            if (price.HasValue) fosterKoi.Price = price.Value;
            if (fosteringDays.HasValue) fosterKoi.FosteringDays = fosteringDays.Value;
            if (consignmentId.HasValue) fosterKoi.ConsignmentID = consignmentId.Value;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(fosterKoi);
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
          [FromQuery] decimal pricePerDay,
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
                Price = pricePerDay,
                FosteringDays = fosteringDays,
                ConsignmentID = consignmentId
            };

            _context.FosterKois.Add(fosterKoi);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateFosterKoi), new { id = fosterKoi.FosterKoiID }, fosterKoi);
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
            var koi = await _context.FosterKois.FindAsync(id);
            if (koi == null)
            {
                return NotFound();
            }

            await fosterKoiService.DeleteKoi(koi);

            return NoContent();
        }

        private bool KoiExists(int id)
        {
            return _context.FosterKois.Any(e => e.FosterKoiID == id);
        }
    }
}
