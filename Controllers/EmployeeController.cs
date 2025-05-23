using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Controllers;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController:ControllerBase {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service) {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Employee emp) {
            var result = await _service.Register(emp);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var user = await _service.Login(login);

            if (user != null)
            {
                // Return only necessary fields (e.g., excluding password)
                return Ok(new
                {
                    employeeId = user.EmployeeId,
                    name = user.Name,
                    email = user.Email,
                    role = user.Role
                });
            }

            return Unauthorized("Invalid credentials");
        }

    }
}