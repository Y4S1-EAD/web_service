using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // POST: api/rating
        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] Ratings rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data", errors = ModelState });
            }

            try
            {
                await _ratingService.AddRatingAsync(rating);
                return Ok(new { message = "Rating added successfully." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the rating.", error = ex.Message });
            }
        }

        // GET: api/rating/vendor/{vendorId}
        [HttpGet("vendor/{vendorId:length(24)}")]
        public async Task<IActionResult> GetRatingsForVendor(string vendorId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByVendorAsync(vendorId);
                return Ok(ratings);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the ratings.", error = ex.Message });
            }
        }

        // GET: api/rating/vendor/{vendorId}/summary
        [HttpGet("vendor/{vendorId:length(24)}/summary")]
        public async Task<IActionResult> GetVendorRatingSummary(string vendorId)
        {
            try
            {
                var summary = await _ratingService.GetVendorRatingSummaryAsync(vendorId);
                return Ok(summary);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the rating summary.", error = ex.Message });
            }
        }
    }
}
