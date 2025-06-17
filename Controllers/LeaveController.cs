using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IConfiguration _configuration;

        public LeaveController(IEmployeeService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaves leave)
        {
            try
            {
                var result = await _service.ApplyLeave(leave);
                if (result == "Leave applied successfully")
                    return Ok(new { message = result });

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        [HttpDelete("cancel/{leaveId}")]
        public async Task<IActionResult> CancelLeave(int leaveId)
        {
            try
            {
                var result = await _service.CancelLeave(leaveId);
                if (result == "Leave cancelled")
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

        // ✅ Unified PUT endpoint that executes the stored procedure directly
        [HttpPut("status")]
        public IActionResult UpdateStatus([FromQuery] int leaveId, [FromQuery] string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateLeaveStatus", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return Ok(new { message = "Leave status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }

}
