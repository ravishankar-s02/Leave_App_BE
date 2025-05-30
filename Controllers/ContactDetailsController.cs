using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/contact-details")]
    public class ContactDetailsController : ControllerBase
    {
        private readonly IContactDetailsService _service;

        public ContactDetailsController(IContactDetailsService service)
        {
            _service = service;
        }

        // ✅ Save contact details
        [HttpPost("save")]
        public async Task<IActionResult> SaveContactDetails([FromBody] ContactDetails details)
        {
            var result = await _service.SaveContactDetails(details);

            if (result)
            {
                // Return a well-formed JSON response with a 200 status
                return Ok(new { message = "Saved successfully" });
            }
            else
            {
                // Return a clear error message with a 500 status
                return StatusCode(500, new { message = "Error saving details" });
            }
        }

        // ✅ Get contact details by employee ID
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetContactDetails(int employeeId)
        {
            var details = await _service.GetContactDetails(employeeId);
            if (details == null)
                return NotFound("No contact details found.");

            return Ok(details);
        }
    }
}
