using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swp_be.Data;
using swp_be.Models;
using swp_be.Services;
using System.Security.Claims;

namespace swp_be.Controllers
{
    public class FeedbackRequest
    {
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class FeedbackUpdateRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly UserService userService;

        public ApplicationDBContext Context { get; }

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            this.userService = new UserService(Context);
        }

        [HttpGet]
        [Authorize("all")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetFeedbacksWithUser();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        [Authorize("all")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackWithUserById(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        [HttpPost("CreateFeedback")]
        [Authorize("all")]
        public async Task<ActionResult<Feedback>> CreateFeedback(FeedbackRequest feedbackRequest)
        {
            var feedback = new Feedback
            {
                CustomerID = feedbackRequest.CustomerID,
                OrderID = feedbackRequest.OrderID,
                Rating = feedbackRequest.Rating,
                Comment = feedbackRequest.Comment,
                DateFb = DateTime.Now
            };

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
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackUpdateRequest updateRequest)
        {
            var updatedFeedback = await _feedbackService.UpdateRatingAndComment(id, updateRequest.Rating, updateRequest.Comment);

            if (updatedFeedback == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        [Authorize("all")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            int userID = int.Parse(User.FindFirstValue("userID"));

            var checkRole = userService.GetUserProfile(userID);

            var feedback = await _feedbackService.GetFeedbackById(id);

            bool deleted = false;

            if (checkRole.Role == Role.Customer)
            {
                if(feedback.CustomerID == userID)
                {
                    deleted = await _feedbackService.DeleteFeedback(id);
                }
            }
            else
            {
                deleted = await _feedbackService.DeleteFeedback(id);
            }

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
