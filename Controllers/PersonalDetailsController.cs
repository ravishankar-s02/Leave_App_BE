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

        [HttpPost("save")]
        public async Task<IActionResult> SavePersonalDetails([FromBody] PersonalDetails details)
        {
            var result = await _service.SavePersonalDetails(details);

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
        public async Task<IActionResult> GetPersonalDetails(int employeeId)
        {
            var details = await _service.GetPersonalDetails(employeeId);
            if (details == null)
                return Ok("No personal details found.");

            return Ok(details);
        }
    }
}
