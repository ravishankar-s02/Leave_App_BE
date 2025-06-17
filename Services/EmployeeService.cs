using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;

namespace LeaveAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<string> Register(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_RegisterEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", emp.MiddleName);
            cmd.Parameters.AddWithValue("@LastName", emp.LastName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Password", emp.Password);
            cmd.Parameters.AddWithValue("@Role", emp.Role);

            try
            {
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return "Registered successfully.";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000) // custom error
                    return ex.Message;

                return "Database error: " + ex.Message;
            }
        }

        public async Task<Employee> Login(LoginRequest login)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_LoginEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", login.FirstName);
            cmd.Parameters.AddWithValue("@Password", login.Password);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Role = reader["Role"].ToString()
                };
            }

            return null;
        }

        public async Task<string> ApplyLeave(ApplyLeaves leave)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ApplyLeave", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TypeName", leave.TypeName);
            cmd.Parameters.AddWithValue("@EmployeeId", leave.EmployeeId);
            cmd.Parameters.AddWithValue("@StartDate", leave.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", leave.EndDate);
            cmd.Parameters.AddWithValue("@Reason", leave.Reason);

            try
            {
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return "Leave applied successfully.";
            }
            catch (SqlException ex)
            {
                return "Database error: " + ex.Message;
            }
        }

        public async Task<string> CancelLeave(int leaveId)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_CancelLeave", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LeaveId", leaveId);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return "Leave cancelled";
        }


        public async Task<List<LeaveApplication>> GetLeavesByEmployee(int empId)
        {
            var list = new List<LeaveApplication>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetLeavesByEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", empId);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LeaveApplication
                {
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"],
                    Reason = reader["Reason"].ToString(),
                    EmployeeId = (int)reader["EmployeeId"],
                    LeaveTypeId = (int)reader["LeaveTypeId"],
                    TypeName = reader["TypeName"].ToString(),
                    Status = reader["Status"].ToString()
                });
            }
            return list;
        }

        public async Task<List<LeaveApplication>> GetAllLeaves()
        {
            var list = new List<LeaveApplication>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAllLeaveApplications", con);
            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LeaveApplication
                {
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"],
                    Reason = reader["Reason"].ToString(),
                    EmployeeId = (int)reader["EmployeeId"],
                    LeaveTypeId = (int)reader["LeaveTypeId"],
                    TypeName = reader["TypeName"].ToString(),
                    Status = reader["Status"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    AppliedOn = (DateTime)reader["AppliedOn"],
                    LeaveId = (int)reader["LeaveId"]
                });
            }
            return list;
        }

        public async Task<string> UpdateLeaveStatus(int leaveId, string status)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateLeaveStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LeaveId", leaveId);
            cmd.Parameters.AddWithValue("@Status", status);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return "Leave status updated.";
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("GetAllEmployees", con);
            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    FirstName = reader["FirstName"].ToString()
                });
            }

            return employees;
        }
    }
}
