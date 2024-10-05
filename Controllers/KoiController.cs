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
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Koi>>> GetKois()
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
                await _context.SaveChangesAsync();
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

            _context.Kois.Remove(koi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KoiExists(int id)
        {
            return _context.Kois.Any(e => e.KoiID == id);
        }
    }
}
