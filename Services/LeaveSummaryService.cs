using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using LeaveAPI.Models;

namespace LeaveAPI.Services
{
    public class LeaveSummaryService : ILeaveSummaryService
    {
        private readonly IConfiguration _configuration;

        public LeaveSummaryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ✅ 1. Get leave summary for a specific employee
        public List<LeaveSummaryModel> GetLeaveSummaryByEmployee(int employeeId)
        {
            List<LeaveSummaryModel> summaries = new List<LeaveSummaryModel>();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_GetLeaveSummaryByEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    summaries.Add(new LeaveSummaryModel
                    {
                        LeaveType = reader["LeaveType"].ToString(),
                        LeaveTaken = Convert.ToDecimal(reader["LeaveTaken"]),
                        LeaveScheduled = Convert.ToDecimal(reader["LeaveScheduled"]),
                        LeaveRemaining = Convert.ToDecimal(reader["LeaveRemaining"])
                    });
                }
            }

            return summaries;
        }

        // ✅ 2. Admin manually uploads leave remaining for any type
        public void UploadLeaveBalance(LeaveSummaryModel model)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_UploadLeaveBalance", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", model.EmployeeID);
                cmd.Parameters.AddWithValue("@Year", model.Year);
                cmd.Parameters.AddWithValue("@TypeName", model.LeaveType);
                cmd.Parameters.AddWithValue("@LeaveRemaining", model.LeaveRemaining);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ✅ 3. Called when status is updated to 'APPROVE' — this will auto-update LeaveScheduled
        public void UpdateLeaveStatus(int leaveId, string status)
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
        }
    }
}
