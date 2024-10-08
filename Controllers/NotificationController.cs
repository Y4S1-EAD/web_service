using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var notifications = await _notificationService.GetAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving notifications.", error = ex.Message });
            }
        }

        // GET: api/Notification/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var notification = await _notificationService.GetAsync(id);

                if (notification == null)
                {
                    return NotFound(new { message = "Notification not found." });
                }

                return Ok(notification);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving the notification.", error = ex.Message });
            }
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification newNotification)
        {
            newNotification.NotificationId = null; // Ensure NotificationId is autogenerated

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                await _notificationService.CreateAsync(newNotification);
                return Ok(new { message = "Notification created successfully.", notification = newNotification });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while creating the notification.", error = ex.Message });
            }
        }

        // PUT: api/Notification/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Notification updatedNotification)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                var notification = await _notificationService.GetAsync(id);

                if (notification == null)
                {
                    return NotFound(new { message = "Notification not found." });
                }

                updatedNotification.NotificationId = notification.NotificationId; // Keep the original NotificationId

                await _notificationService.UpdateAsync(id, updatedNotification);

                return Ok(new { message = "Notification updated successfully.", notification = updatedNotification });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while updating the notification.", error = ex.Message });
            }
        }

        // DELETE: api/Notification/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var notification = await _notificationService.GetAsync(id);

                if (notification == null)
                {
                    return NotFound(new { message = "Notification not found." });
                }

                await _notificationService.RemoveAsync(id);

                return Ok(new { message = "Notification deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while deleting the notification.", error = ex.Message });
            }
        }
    }
}
