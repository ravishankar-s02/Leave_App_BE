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

        [HttpPost("save")]
        public async Task<IActionResult> SaveSalary([FromBody] Salary details)
        {
            var result = await _service.SaveSalary(details);

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
        public async Task<IActionResult> GetSalary(int employeeId)
        {
            var details = await _service.GetSalary(employeeId);
            if (details == null)
                return Ok("No Salary details found.");

            return Ok(details);
        }
    }
}
