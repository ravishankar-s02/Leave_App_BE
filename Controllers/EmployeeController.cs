using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;
using System.Data.SqlClient; // Make sure this is included
using Microsoft.Extensions.Configuration;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public EmployeeController(IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Employee emp)
        {
            var result = await _employeeService.Register(emp);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var user = await _employeeService.Login(login);

            if (user != null)
            {
                return Ok(new
                {
                    employeeId = user.EmployeeId,
                    firstName = user.FirstName,
                    email = user.Email,
                    role = user.Role
                });
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                string token = Guid.NewGuid().ToString();

                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Employees SET ResetToken = @token WHERE Email = @Email", conn);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        // In real app: send token via email
                        return Ok(new { message = "Reset link sent", token });
                    }

                    return BadRequest(new { message = "Email not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Employees SET Password = @Password, ResetToken = NULL WHERE ResetToken = @Token", conn);
                    cmd.Parameters.AddWithValue("@Password", model.NewPassword); // 🔐 Hashing recommended
                    cmd.Parameters.AddWithValue("@Token", model.Token);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok(new { message = "Password reset successful" });

                    return BadRequest(new { message = "Invalid or expired token" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }
    }
}
