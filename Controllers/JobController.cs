using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _service;

        public JobController(IJobService service)
        {
            _service = service;
        }

        // ✅ Save contact details
        [HttpPost("save")]
        public async Task<IActionResult> SaveJob([FromBody] Job details)
        {
            var result = await _service.SaveJob(details);

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
        public async Task<IActionResult> GetJob(int employeeId)
        {
            var details = await _service.GetJob(employeeId);
            if (details == null)
                return Ok("No contact details found.");

            return Ok(details);
        }
    }
}
