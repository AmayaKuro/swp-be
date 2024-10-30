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
using swp_be.Utils;

namespace swp_be.Controllers
{
    public class KoiRequest
    {
        public int? KoiID { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? DailyFeedAmount { get; set; }
        public long Price { get; set; }
        public string? Personality { get; set; }
        public string? Origin { get; set; }
        public string? SelectionRate { get; set; }
        public string Species { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? OriginCertificate { get; set; }  // Gi?y ngu?n g?c xu?t x?
        public IFormFile? HealthCertificate { get; set; }  // Gi?y ki?m tra s?c kh?e
        public IFormFile? OwnershipCertificate { get; set; }  // Gi?y ch?ng nh?n cá Koi
    }

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
        public async Task<ActionResult<Koi>> GetKoi()
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

        // GET: api/koi/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableKois()
        {
            var availableKois = await koiService.GetAvailableKoisAsync();
            return Ok(availableKois);
        }


        // PUT: api/Koi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
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
        [Authorize("staff, admin")]
        [HttpPost]
        // TODO: Set this to FromBody later
        public async Task<ActionResult<Koi>> PostKoi([FromForm] KoiRequest koiRequest)
        {
            var fbUtils = new FirebaseUtils();

            // Prepare image URL
            IFormFile koiImage = null, originCertificate = null, healthCertificate = null, ownershipCertificate = null;

            // THIS IS ROLL BACK CODE
            //// Categorize image from image list by name
            //foreach (var img in koiRequest.Image)
            //{
            //    if (img.ContentType.ToLower().StartsWith("image/"))
            //    {
            //        Console.WriteLine("Namee " + img.Name);
            //        Console.WriteLine("File Namee " + img.FileName);
            //        switch (img.FileName.Split(".")[0])
            //        {
            //            case "koiImage":
            //                koiImage = img;
            //                break;
            //            case "originCertificate":
            //                originCertificate = img;
            //                break;
            //            case "healthCertificate":
            //                healthCertificate = img;
            //                break;
            //            case "ownershipCertificate":
            //                ownershipCertificate = img;
            //                break;
            //        }
            //    }
            //}

            var koi = new Koi
            {
                Name = koiRequest.Name,
                Gender = koiRequest.Gender,
                Age = koiRequest.Age,
                Size = koiRequest.Size,
                Color = koiRequest.Color,
                DailyFeedAmount = koiRequest.DailyFeedAmount,
                Price = koiRequest.Price,
                Personality = koiRequest.Personality,
                SelectionRate = koiRequest.SelectionRate,
                Species = koiRequest.Species,
                Origin = koiRequest.Origin,
                //Image = imageUrl,
                AddOn = new AddOn(),
                //{
                //    OriginCertificate = originCertificateUrl,
                //    HealthCertificate = healthCertificateUrl,
                //    OwnershipCertificate = ownershipCertificateUrl
                //},
            };
            await koiService.CreateKoi(koi);

            // Add image base on input
            koi.Image = await fbUtils.UploadImage(koiRequest.Image?.OpenReadStream(), koi.KoiID.ToString(), "koiImage");
            koi.AddOn.OriginCertificate = await fbUtils.UploadImage(koiRequest.OriginCertificate?.OpenReadStream(), koi.KoiID.ToString(), "originCertificate");
            koi.AddOn.HealthCertificate = await fbUtils.UploadImage(koiRequest.HealthCertificate?.OpenReadStream(), koi.KoiID.ToString(), "healthCertificate");
            koi.AddOn.OwnershipCertificate = await fbUtils.UploadImage(koiRequest.OwnershipCertificate?.OpenReadStream(), koi.KoiID.ToString(), "ownershipCertificate");

            // Update to add the rest  of imagee into Koi
            koiService.UpdateKoi(koi);

            return CreatedAtAction("GetKoi", new { id = koi.KoiID }, koi);
        }

        // DELETE: api/Koi/5
        [Authorize("staff, admin")]
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
                kois = kois.Where(k => k.Price >= minPrice);

            if (maxPrice.HasValue)
                kois = kois.Where(k => k.Price <= maxPrice);

            if (!string.IsNullOrEmpty(species))
                kois = kois.Where(k => k.Species.Contains(species, StringComparison.OrdinalIgnoreCase));

            return Ok(kois.ToList());
        }

    }
}
