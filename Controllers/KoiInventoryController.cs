using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Models;
using swp_be.Services;
using System.Security.Claims;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiInventoryController : ControllerBase
    {
        private readonly KoiInventoryService koiInventoryService;

        public KoiInventoryController(KoiInventoryService koiInventoryService)
        {
            this.koiInventoryService = koiInventoryService;
        }

        // GET: api/KoiInventory
        [HttpGet]
        [Authorize("staff, admin")]
        public async Task<ActionResult<List<KoiInventory>>> GetKoiInventory()
        {
            var inventory = await koiInventoryService.GetKoiInventoryAsync();
            return Ok(inventory);
        }

        // GET: api/KoiInventory/5
        [HttpGet("{id}")]
        [Authorize("staff, admin")]
        public async Task<ActionResult<KoiInventory>> GetKoiInventoryById(int id)
        {
            var koiInventory = await koiInventoryService.GetKoiInventoryByIdAsync(id);
            if (koiInventory == null)
            {
                return NotFound();
            }

            return Ok(koiInventory);
        }

        // GET: api/KoiInventory/userId
        [HttpGet("userKoiInventory")]
        [Authorize("all")]
        public async Task<ActionResult<List<KoiInventory>>> GetKoiInventoryByUserId()
        {
            int customerID = int.Parse(User.FindFirstValue("userID"));
            if (customerID == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var koiInventoryList = await koiInventoryService.GetKoiInventoryByUserIdAsync(customerID);

            return Ok(koiInventoryList);
        }

        [HttpPost("CreateFromKoi")]
        [Authorize("all")]
        public async Task<ActionResult<KoiInventory>> CreateKoiInventoryFromKoi([FromQuery] int koiId, [FromQuery] bool isConsignment)
        {
            try
            {
                int customerID = int.Parse(User.FindFirstValue("userID"));
                var koiInventory = await koiInventoryService.CreateKoiInventoryFromKoiAsync(koiId, customerID, isConsignment);
                return CreatedAtAction(nameof(GetKoiInventoryById), new { id = koiInventory.KoiInventoryID }, koiInventory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/KoiInventory/5
        [HttpPut("{id}")]
        [Authorize("all")]
        public async Task<IActionResult> PutKoiInventory(int id, KoiInventory koiInventory)
        {
            if (id != koiInventory.KoiInventoryID)
            {
                return BadRequest();
            }

            var updatedKoiInventory = await koiInventoryService.UpdateKoiInventoryAsync(koiInventory);
            return Ok(updatedKoiInventory);
        }

        // DELETE: api/KoiInventory/5
        [HttpDelete("{id}")]
        [Authorize("all")]
        public async Task<IActionResult> DeleteKoiInventory(int id)
        {
            var koiInventory = await koiInventoryService.GetKoiInventoryByIdAsync(id);
            if (koiInventory == null)
            {
                return NotFound();
            }

            await koiInventoryService.DeleteKoiInventoryAsync(koiInventory);
            return NoContent();
        }
    }
}
