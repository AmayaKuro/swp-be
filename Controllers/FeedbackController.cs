using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Models;
using swp_be.Services;

namespace swp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedback
        [HttpGet]
        [Authorize("all")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetFeedbacks();
            return Ok(feedbacks);
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        [Authorize("all")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        // POST: api/Feedback
        [HttpPost]
        [Authorize("all")]
        public async Task<ActionResult<Feedback>> CreateFeedback(Feedback feedback)
        {
/*            var createdFeedback = await _feedbackService.CreateFeedback(feedback);
            return CreatedAtAction(nameof(GetFeedback), new { id = createdFeedback.FeedbackID }, createdFeedback);*/

            var createdFeedback = await _feedbackService.CreateFeedback(feedback);

            if (createdFeedback == null)
            {
                return BadRequest("Order does not exist or has not been completed yet.");
            }

            return CreatedAtAction(nameof(GetFeedback), new { id = createdFeedback.FeedbackID }, createdFeedback);

        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        [Authorize("all")]
        public async Task<IActionResult> UpdateFeedback(int id, Feedback feedback)
        {
            if (id != feedback.FeedbackID)
            {
                return BadRequest();
            }

            var updatedFeedback = await _feedbackService.UpdateFeedback(feedback);

            if (updatedFeedback == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        [Authorize("admin")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var deleted = await _feedbackService.DeleteFeedback(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("search")]
        [Authorize("all")]
        public async Task<IActionResult> SearchFeedbacks([FromQuery] int? rating, [FromQuery] DateTime? dateFb)
        {
            var feedbacks = await _feedbackService.SearchFeedbacks(rating, dateFb);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound();
            }
            return Ok(feedbacks);
        }
    }
}
