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

        [HttpPost("save")]
        public async Task<IActionResult> SaveJob([FromBody] Job details)
        {
            var result = await _service.SaveJob(details);

            if (result)
            {
                return Ok(new { message = "Saved successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "Error saving details" });
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetJob(int employeeId)
        {
            var details = await _service.GetJob(employeeId);
            if (details == null)
                return Ok("No job details found.");

            return Ok(details);
        }
    }
}
