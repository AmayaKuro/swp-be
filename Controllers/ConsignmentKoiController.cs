﻿using Microsoft.AspNetCore.Authorization;
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
        public string? species { get; set; }
        public long? price { get; set; }
        public int? consignmentId { get; set; }
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
            if (consignKoi.price < 0)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            // Find the foster koi, ensuring you await the result
            var info = await ConsignmentKoiservice.GetById(id);

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
            if (consignKoi.price >= 0) info.Price = consignKoi.price ?? info.Price; 
            if (consignKoi.consignmentId > 0) info.ConsignmentID = consignKoi.consignmentId ?? info.ConsignmentID;
            await _context.SaveChangesAsync();
            // Update image
            // Add image base on input
            if (consignKoi.Image != null)
            {
                info.Image = await fbUtils.UploadImage(consignKoi.Image.OpenReadStream(), info.ConsignmentKoiID.ToString(), "koiImage");
            }
            try
            {
                if (consignKoi.OriginCertificate != null)
                {
                    info.AddOn.OriginCertificate = await fbUtils.UploadImage(consignKoi.OriginCertificate.OpenReadStream(), info.ConsignmentKoiID.ToString(), "originCertificate");
                }
                if (consignKoi.HealthCertificate != null)
                {
                    info.AddOn.HealthCertificate = await fbUtils.UploadImage(consignKoi.HealthCertificate.OpenReadStream(), info.ConsignmentKoiID.ToString(), "healthCertificate");
                }
                if (consignKoi.OwnershipCertificate != null)
                {
                    info.AddOn.OwnershipCertificate = await fbUtils.UploadImage(consignKoi.OwnershipCertificate.OpenReadStream(), info.ConsignmentKoiID.ToString(), "ownershipCertificate");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(info);
          
        }



        // POST: api/Koi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize("staff, admin")]
        [HttpPost("CreateFosterKoi")]
        public async Task<IActionResult> CreateConsignKoi([FromForm] ConsignKoiReq consignKoi)
        {
            if (consignKoi.price < 0)
            {
                return BadRequest(new { message = "Wrong input format" });
            }

            if (string.IsNullOrWhiteSpace(consignKoi.species) || consignKoi.species.Length > 255)
            {
                return BadRequest("Invalid species.");
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
                Price = consignKoi.price ?? 1,  // Defaulting to 0 if null
                ConsignmentID = consignKoi.consignmentId ?? 0,  // Defaulting to 0 if null
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
