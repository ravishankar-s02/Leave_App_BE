using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
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
    }
}
