using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/salary")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _service;

        public SalaryController(ISalaryService service)
        {
            _service = service;
        }

        // ✅ Save contact details
        [HttpPost("save")]
        public async Task<IActionResult> SaveSalary([FromBody] Salary details)
        {
            var result = await _service.SaveSalary(details);

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
        public async Task<IActionResult> GetSalary(int employeeId)
        {
            var details = await _service.GetSalary(employeeId);
            if (details == null)
                return Ok("No contact details found.");

            return Ok(details);
        }
    }
}
