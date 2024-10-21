using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;

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
            await promotionService.CreatePromotion(promotion);
            return CreatedAtAction(nameof(GetPromotion), new { id = promotion.PromotionID }, promotion);
        }

        // PUT: api/Promotion/5
        [HttpPut("{id}")]
        [Authorize("admin")]
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
        [Authorize("admin")]
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
        public async Task<ActionResult<IEnumerable<Promotion>>> SearchPromotions([FromQuery] string? code, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return Ok(await promotionService.SearchPromotions(code, startDate, endDate));
        }
    }
}
