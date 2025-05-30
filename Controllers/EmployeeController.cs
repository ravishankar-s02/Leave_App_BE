using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        // Use one private field only
        private readonly IEmployeeService _employeeService;

        // Single constructor
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

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
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
                    name = user.Name,
                    email = user.Email,
                    role = user.Role
                });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
