using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp_be.Data;
using swp_be.Data.Repositories;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/koi/[controller]")]
    [ApiController]
    public class KoiController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly KoiService koiService;

        public KoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.koiService = new KoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> Get()
        {
            return Ok(await koiService.GetKois());
            //return await _context.Kois.Take(10).ToListAsync();
        }

        // GET: api/Koi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Koi>> GetKoi(int id)
        {
            var koi = await _context.Kois.FindAsync(id);

            if (koi == null)
            {
                return NotFound();
            }

            return koi;
        }


        // PUT: api/Koi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKoi(int id, Koi koi)
        {
            if (id != koi.KoiID)
            {
                return BadRequest();
            }

            _context.Entry(koi).State = EntityState.Modified;

            try
            {
                await koiService.UpdateKoi(koi);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KoiExists(id))
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
        [HttpPost]
        public async Task<ActionResult<Koi>> PostKoi(Koi koi)
        {
            await koiService.CreateKoi(koi);

            return CreatedAtAction("GetKoi", new { id = koi.KoiID }, koi);
        }

        // DELETE: api/Koi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKoi(int id)
        {
            var koi = await _context.Kois.FindAsync(id);
            if (koi == null)
            {
                return NotFound();
            }

            await koiService.DeleteKoi(koi);

            return NoContent();
        }
       
        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<Koi>>> GetSortedKois()
        {
            var kois = await koiService.GetKois(); // Fetch the Kois
          
            return Ok(kois.OrderBy(k => k.Name).ToList()); // Sort by Name and return
                                                           // ... existing code ...
        }
        // GET: api/Koi?search=someValue
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Koi>>> GetKois(string? search = null)
        {
            var kois = await koiService.GetKois(); // Fetch the Kois

            if (!string.IsNullOrEmpty(search))
            {
                kois = kois.Where(k =>
                    k.Name != null && k.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    k.Color != null && k.Color.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    k.Personality != null && k.Personality.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    k.Species != null && k.Species.Contains(search, StringComparison.OrdinalIgnoreCase)
                // Add more properties to search as needed
                );
            }

            return Ok(kois.ToList()); // Return the filtered list
                                      // ... existing code ...
        }
        private bool KoiExists(int id)
        {
            return _context.Kois.Any(e => e.KoiID == id);
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<Koi>>> FilterKois(
            [FromQuery] string? name,
            [FromQuery] string? gender,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] string? size,
            [FromQuery] string? color,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? species)
        {
            var kois = await koiService.GetKois();

            if (!string.IsNullOrEmpty(name))
                kois = kois.Where(k => k.Name != null && k.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(gender))
                kois = kois.Where(k => k.Gender != null && k.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            if (minAge.HasValue)
                kois = kois.Where(k => k.Age.HasValue && k.Age >= minAge);

            if (maxAge.HasValue)
                kois = kois.Where(k => k.Age.HasValue && k.Age <= maxAge);

            if (!string.IsNullOrEmpty(size))
                kois = kois.Where(k => k.Size != null && k.Size.Contains(size, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(color))
                kois = kois.Where(k => k.Color != null && k.Color.Contains(color, StringComparison.OrdinalIgnoreCase));

            if (minPrice.HasValue)
                kois = kois.Where(k => k.Price.HasValue && k.Price >= minPrice);

            if (maxPrice.HasValue)
                kois = kois.Where(k => k.Price.HasValue && k.Price <= maxPrice);

            if (!string.IsNullOrEmpty(species))
                kois = kois.Where(k => k.Species.Contains(species, StringComparison.OrdinalIgnoreCase));

            return Ok(kois.ToList());
        }

    }
}
