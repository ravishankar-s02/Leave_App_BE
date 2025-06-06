using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/personal-details")]
    public class PersonalDetailsController : ControllerBase
    {
        private readonly IPersonalDetailsService _service;

        public PersonalDetailsController(IPersonalDetailsService service)
        {
            _service = service;
        }

        // ✅ Save personal details
        [HttpPost("save")]
        public async Task<IActionResult> SavePersonalDetails([FromBody] PersonalDetails details)
        {
            var result = await _service.SavePersonalDetails(details);

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


        // ✅ Get personal details by employee ID
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetPersonalDetails(int employeeId)
        {
            var details = await _service.GetPersonalDetails(employeeId);
            if (details == null)
                return NotFound("No personal details found.");

            return Ok(details);
        }
    }
}
