using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public LeaveController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaves leave)
        {
            try
            {
                var result = await _service.ApplyLeave(leave); // assume returns string message

                if (result == "Leave applied successfully")
                    return Ok(new { message = result });

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }


        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetLeavesByEmployee(int id)
        {
            var leaves = await _service.GetLeavesByEmployee(id);
            return Ok(leaves);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var leaves = await _service.GetAllLeaves();
            return Ok(leaves);
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int leaveId, [FromQuery] string status)
        {
            try
            {
                var result = await _service.UpdateLeaveStatus(leaveId, status); // result is string

                if (result == "Status updated successfully")
                    return Ok(new { message = result });

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

    }
}
