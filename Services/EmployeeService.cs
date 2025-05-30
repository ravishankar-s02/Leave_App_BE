using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Services
{
    public class EmployeeService(IConfiguration config) : IEmployeeService
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection");

        public async Task<string> Register(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_RegisterEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", emp.Name);
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
                if (ex.Number == 50000) // Matches RAISERROR in stored proc
                {
                    return ex.Message; // Returns: "Email already exists."
                }

                return "Database error: " + ex.Message;
            }
        }

        public async Task<Employee> Login(LoginRequest login)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_LoginEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", login.Name);
            cmd.Parameters.AddWithValue("@Password", login.Password);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    Name = reader["Name"].ToString(),
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
                return "Leave applied successfully";
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
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
                    Name = reader["Name"].ToString(),
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

        public Task<string> ApplyLeave(LeaveApplication leave)
        {
            throw new NotImplementedException();
        }

        public Task<string> ApplyLeave(object leaves)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetAllEmployees", conn); // or SELECT * FROM Employees
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        Name = reader["Name"].ToString()
                        // Add more fields if needed
                    });
                }
            }
        }

        return employees;
    }
        
        
    }
}