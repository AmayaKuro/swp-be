using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using swp_be.Utils;
using System.Drawing;
using System.Security.Claims;

namespace swp_be.Controllers
{
    public class ConsignKoiReq
    {
        public int? ConsignmentKoiID { get; set; }
        public string? name { get; set; }
        public string? gender { get; set; }
        public int? age { get; set; }
        public string? size { get; set; }
        public string? color { get; set; }
        public string? dailyFeedAmount { get; set; }
        public string? personality { get; set; }
        public string? origin { get; set; }
        public string? selectionRate { get; set; }
        public string species { get; set; }
        public long pricePerDay { get; set; }
        public int fosteringDays { get; set; }
        public int consignmentId { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? OriginCertificate { get; set; }
        public IFormFile? HealthCertificate { get; set; }
        public IFormFile? OwnershipCertificate { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentKoiController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ConsignmentKoiService ConsignmentKoiservice;
        private readonly FirebaseUtils fbUtils = new FirebaseUtils();

        public ConsignmentKoiController(ApplicationDBContext context)
        {
            this._context = context;
            this.ConsignmentKoiservice = new ConsignmentKoiService(context);
        }

        // GET: api/Koi
        [HttpGet]
        public async Task<ActionResult<Koi>> GetConsignmentKoi()
        {
            return Ok(await ConsignmentKoiservice.GetConsignmentKois());
        }

        // GET: api/Koi/5        
        [HttpGet("{id}")]
        [Authorize("all")]
        public async Task<ActionResult<ConsignmentKoi>> GetConsignmentKoi(int id)
        {
            var consignmentKoi = await ConsignmentKoiservice.GetConsignmentKoi(id);

            if (consignmentKoi == null)
            {
                return NotFound();
            }

            return consignmentKoi;
        }

        [HttpGet("GetConsignmentKoisByUserID")]
        public async Task<ActionResult<List<ConsignmentKoi>>> GetConsignmentKoisByUser()
        {
            int userID = int.Parse(User.FindFirstValue("userID"));
            var consignmentKois = await ConsignmentKoiservice.GetConsignmentKoisByUser(userID);
            if (consignmentKois == null || !consignmentKois.Any())
            {
                return NotFound("No consignment koi found for the given user ID.");
            }
            return Ok(consignmentKois);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFosterKoi(int id, [FromForm] ConsignKoiReq consignKoi)
        {
            // Find the foster koi, ensuring you await the result
            var info = await _context.ConsignmentKois.FindAsync(id);

            // Check if foster koi exists
            if (info == null)
            {
                return NotFound();
            }

            // Validate species if provided
            if (!string.IsNullOrWhiteSpace(consignKoi.species) && consignKoi.species.Length > 255)
            {
                return BadRequest("Invalid species.");
            }

            // Validate fostering days if provided
            if (consignKoi.fosteringDays <= 0)
            {
                return BadRequest("Fostering days must be greater than zero.");
            }

            // Update foster koi properties only if they are provided
            if (consignKoi.name != null) info.Name = consignKoi.name;
            if (consignKoi.gender != null) info.Gender = consignKoi.gender;
            if (consignKoi.age.HasValue) info.Age = consignKoi.age.Value;
            // Update foster koi properties only if they are provided
            if (consignKoi.name != null) info.Name = consignKoi.name;
            if (consignKoi.gender != null) info.Gender = consignKoi.gender;
            if (consignKoi.age.HasValue) info.Age = consignKoi.age.Value;
            if (consignKoi.size != null) info.Size = consignKoi.size;
            if (consignKoi.color != null) info.Color = consignKoi.color;
            if (consignKoi.dailyFeedAmount != null) info.DailyFeedAmount = consignKoi.dailyFeedAmount;
            if (consignKoi.personality != null) info.Personality = consignKoi.personality;
            if (consignKoi.origin != null) info.Origin = consignKoi.origin;
            if (consignKoi.selectionRate != null) info.SelectionRate = consignKoi.selectionRate;
            if (consignKoi.species != null) info.Species = consignKoi.species;
            if (consignKoi.pricePerDay >= 0) info.Price = consignKoi.pricePerDay;
            if (consignKoi.fosteringDays >= 0) info.FosteringDays = consignKoi.fosteringDays;
            if (consignKoi.consignmentId > 0) info.ConsignmentID = consignKoi.consignmentId;

            // Update image
            // Add image base on input
            info.Image = await fbUtils.UploadImage(consignKoi.Image?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "koiImage");
            info.AddOn.OriginCertificate = await fbUtils.UploadImage(consignKoi.OriginCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "originCertificate");
            info.AddOn.HealthCertificate = await fbUtils.UploadImage(consignKoi.HealthCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "healthCertificate");
            info.AddOn.OwnershipCertificate = await fbUtils.UploadImage(consignKoi.OwnershipCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "ownershipCertificate");


            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(info);
            //if (size != null) consignmentKoi.Size = size;
            //if (color != null) consignmentKoi.Color = color;
            //if (dailyFeedAmount != null) consignmentKoi.DailyFeedAmount = dailyFeedAmount;
            //if (personality != null) consignmentKoi.Personality = personality;
            //if (origin != null) consignmentKoi.Origin = origin;
            //if (selectionRate != null) consignmentKoi.SelectionRate = selectionRate;
            //if (species != null) consignmentKoi.Species = species;
            //if (price.HasValue) consignmentKoi.Price = price.Value;
            //if (fosteringDays.HasValue) consignmentKoi.FosteringDays = fosteringDays.Value;
            //if (consignmentId.HasValue) consignmentKoi.ConsignmentID = consignmentId.Value;

            // Save the changes to the database
            //await _context.SaveChangesAsync();

            //return Ok(consignmentKoi);
        }



        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
        [HttpPost("CreateFosterKoi")]
        public async Task<IActionResult> CreateConsignKoi([FromForm] ConsignKoiReq consignKoi)
        {
            if (string.IsNullOrWhiteSpace(consignKoi.species) || consignKoi.species.Length > 255)
            {
                return BadRequest("Invalid species.");
            }

            if (consignKoi.fosteringDays <= 0)
            {
                return BadRequest("Fostering days must be greater than zero.");
            }

            var info = new ConsignmentKoi
            {
                Name = consignKoi.name,
                Gender = consignKoi.gender,
                Age = consignKoi.age,
                Size = consignKoi.size,
                Color = consignKoi.color,
                DailyFeedAmount = consignKoi.dailyFeedAmount,
                Personality = consignKoi.personality,
                Origin = consignKoi.origin,
                SelectionRate = consignKoi.selectionRate,
                Species = consignKoi.species,
                Price = consignKoi.pricePerDay,
                FosteringDays = consignKoi.fosteringDays,
                ConsignmentID = consignKoi.consignmentId,
                AddOn = new AddOn(),
            };

            _context.ConsignmentKois.Add(info);

            try
            {
                await _context.SaveChangesAsync();

                // Add image base on input
                info.Image = await fbUtils.UploadImage(consignKoi.Image?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "koiImage");
                info.AddOn.OriginCertificate = await fbUtils.UploadImage(consignKoi.OriginCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "originCertificate");
                info.AddOn.HealthCertificate = await fbUtils.UploadImage(consignKoi.HealthCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "healthCertificate");
                info.AddOn.OwnershipCertificate = await fbUtils.UploadImage(consignKoi.OwnershipCertificate?.OpenReadStream(), info.ConsignmentKoiID.ToString(), "ownershipCertificate");

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateConsignKoi), new { id = info.ConsignmentKoiID }, info);
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
