using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;

public class JobService : IJobService
{
    private readonly IConfiguration _config;

    public JobService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> SaveJob(Job details)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_SaveOrUpdateJob", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", details.EmployeeId);
                cmd.Parameters.AddWithValue("@JobTitle", details.JobTitle);
                cmd.Parameters.AddWithValue("@EmploymentStatus", details.EmploymentStatus);
                cmd.Parameters.AddWithValue("@JoinedDate", details.JoinedDate);
                cmd.Parameters.AddWithValue("@Location", details.Location);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
        }
    }

    public async Task<Job?> GetJob(int employeeId)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_GetJob", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Job
                        {
                            EmployeeId = (int)reader["EmployeeId"],
                            JobTitle = reader["JobTitle"]?.ToString(),
                            EmploymentStatus = reader["EmploymentStatus"]?.ToString(),
                            JoinedDate = (DateTime)reader["JoinedDate"],
                            Location = reader["Location"]?.ToString()
                        };
                    }
                }
            }
        }

        return null; 
    }
}
