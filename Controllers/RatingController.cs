using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

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

        // GET: api/rating/user/{userId}
        [HttpGet("user/{userId:length(24)}")]
        public async Task<IActionResult> GetRatingsForUser(string userId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByUserAsync(userId);
                return Ok(ratings);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the ratings.", error = ex.Message });
            }
        }

       // PATCH: api/rating/{ratingId}
        [HttpPatch("{ratingId:length(24)}")]
        public async Task<IActionResult> PatchRating(string ratingId, [FromBody] JsonPatchDocument<Ratings> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(new { message = "Invalid patch document." });
            }

            // Retrieve the existing rating
            var rating = await _ratingService.GetRatingByIdAsync(ratingId);
            if (rating == null)
            {
                return NotFound(new { message = "Rating not found." });
            }

            // Apply the patch to the existing rating
            patchDoc.ApplyTo(rating, ModelState);

            // Check if the patch operation caused any validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Save the updated rating
            var isUpdated = await _ratingService.UpdateRatingAsync(ratingId, rating);
            if (isUpdated)
            {
                return Ok(new { message = "Rating updated successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "An error occurred while updating the rating." });
            }
        }


    }
}
