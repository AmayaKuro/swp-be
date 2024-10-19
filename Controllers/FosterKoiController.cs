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
    public class FosterKoiController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly FosterKoiService fosterKoiService;

        public FosterKoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.fosterKoiService = new FosterKoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> GetFosterKoi()
        {
            return Ok(await fosterKoiService.GetFosterKois());
        }

        // GET: api/Koi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FosterKoi>> GetFosterKoi(int id)
        {
            var fosterKoi = await _context.FosterKois.FindAsync(id);

            if (fosterKoi == null)
            {
                return NotFound();
            }

            return fosterKoi;
        }


        // PUT: api/Koi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateFosterKoi(
    [FromQuery] int? id,
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

            // Find the foster koi, ensuring you await the result
            var fosterKoi = await _context.FosterKois.FindAsync(id);

            // Check if foster koi exists
            if (fosterKoi == null)
            {
                return NotFound();
            }

            // Update foster koi properties
            fosterKoi.Name = name;
            fosterKoi.Gender = gender;
            fosterKoi.Age = age;
            fosterKoi.Size = size;
            fosterKoi.Color = color;
            fosterKoi.DailyFeedAmount = dailyFeedAmount;
            fosterKoi.Personality = personality;
            fosterKoi.Origin = origin;
            fosterKoi.SelectionRate = selectionRate;
            fosterKoi.Species = species;
            fosterKoi.PricePerDay = pricePerDay;
            fosterKoi.FosteringDays = fosteringDays;
            fosterKoi.ConsignmentID = consignmentId;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(fosterKoi);
        }



        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

            var fosterKoi = new FosterKoi
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
                PricePerDay = pricePerDay,
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
