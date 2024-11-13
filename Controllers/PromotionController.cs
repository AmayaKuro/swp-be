using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using System.Security.Claims;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionService promotionService;

        public PromotionController(ApplicationDBContext context)
        {
            promotionService = new PromotionService(context);
        }

        // GET: api/Promotion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions()
        {
            return Ok(await promotionService.GetPromotions());
        }

        // GET: api/Promotion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Promotion>> GetPromotion(int id)
        {
            var promotion = await promotionService.GetPromotions();
            var foundPromotion = promotion.FirstOrDefault(p => p.PromotionID == id);

            if (foundPromotion == null)
            {
                return NotFound();
            }

            return Ok(foundPromotion);
        }

        // POST: api/Promotion
        [HttpPost]
        [Authorize("all")]
        public async Task<ActionResult<Promotion>> CreatePromotion(Promotion promotion)
        {
            try
            {
                await promotionService.CreatePromotion(promotion);
                return CreatedAtAction(nameof(GetPromotion), new { id = promotion.PromotionID }, promotion);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        // PUT: api/Promotion/5
        [HttpPut("{id}")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> UpdatePromotion(int id, Promotion promotion)
        {
            if (id != promotion.PromotionID)
            {
                return BadRequest();
            }

            await promotionService.UpdatePromotion(promotion);

            return NoContent();
        }

        // DELETE: api/Promotion/5
        [HttpDelete("{id}")]
        [Authorize("staff, admin")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promotions = await promotionService.GetPromotions();
            var promotion = promotions.FirstOrDefault(p => p.PromotionID == id);
            if (promotion == null)
            {
                return NotFound();
            }

            await promotionService.DeletePromotion(promotion);
            return NoContent();
        }

        // GET: api/Promotion/Search
        [HttpGet("Search")]
        [Authorize("all")]
        public async Task<ActionResult<IEnumerable<Promotion>>> SearchPromotions([FromQuery] string? code, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return Ok(await promotionService.SearchPromotions(code, startDate, endDate));
        }

        [HttpPost("Redeem")]
        [Authorize("customer")]
        public async Task<IActionResult> RedeemPromotion()
        {
            try
            {
                int customerId = int.Parse(User.FindFirstValue("userID"));
                var promotion = await promotionService.RedeemPromotion(customerId);
                return Ok(new
                {
                    message = "Redeemed promotion successfully",
                    promotion
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Promotion/UserAvailable
        [HttpGet("UserAvailable")]
        [Authorize("customer")]
        public async Task<IActionResult> GetUserAvailablePromotions()
        {
            int customerId = int.Parse(User.FindFirstValue("userID"));
            var promotions = await promotionService.GetUserAvailablePromotions(customerId);
            return Ok(promotions);
        }
    }
}
