using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveAPI.Models;
using LeaveAPI.Services;


namespace LeaveAPI.Services
{
    public interface IEmployeeService
    {
        Task<string> Register(Employee employee);
        Task<Employee> Login(LoginRequest login);
        Task<string> ApplyLeave(ApplyLeaves leave);
        Task<List<LeaveApplication>> GetLeavesByEmployee(int empId);
        Task<List<LeaveApplication>> GetAllLeaves();
        Task<string> UpdateLeaveStatus(int LeaveId, string status);
    }
}